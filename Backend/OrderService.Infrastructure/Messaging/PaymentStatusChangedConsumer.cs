using BuildingBlocks.Events;
using BuildingBlocks.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Entities.Carts;
using OrderService.Domain.Enums;
using System.Text.Json;
namespace OrderService.Infrastructure.Messaging
{
    public class PaymentStatusChangedConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IConfiguration _configuration;

        public PaymentStatusChangedConsumer(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            // عشان ما نحجزش بدء تشغيل باقي الـ HostedServices
            await Task.Yield();

            Console.WriteLine("🔥 PaymentStatusChangeConsumer STARTED");

            var kafkaConnection = _configuration.GetConnectionString("kafka");
            Console.WriteLine($"🔌 Consumer Kafka Connection: {kafkaConnection}");

            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaConnection,
                GroupId = "order-service",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer =
                new ConsumerBuilder<string, string>(config)
                    .SetErrorHandler((_, e) =>
                        Console.WriteLine($"❌ Kafka error: {e.Reason}"))
                    .Build();

            consumer.Subscribe("payment-status-changed");

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    Console.WriteLine("📩 MESSAGE RECEIVED FROM KAFKA");
                    Console.WriteLine($"Topic: {result.Topic}");
                    Console.WriteLine($"Partition: {result.Partition}");
                    Console.WriteLine($"Offset: {result.Offset}");
                    Console.WriteLine($"Value: {result.Message.Value}");

                    var paymentEvent =
                        JsonSerializer.Deserialize<PaymentStatusChangedEvent>(
                            result.Message.Value,
                            jsonOptions);

                    if (paymentEvent is null)
                    {
                        consumer.Commit(result);
                        continue;
                    }

                    using var scope = _scopeFactory.CreateScope();

                    var orderRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IOrderRepository>();

                    var cartRepository =
                        scope.ServiceProvider
                            .GetRequiredService<ICartRepository>();

                    var orderSagaRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IOrderSagaRepository>();

                    var outboxRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IOutboxRepository>();

                    var processedEventRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IProcessedEventRepository>();

                    // Idempotency
                    var alreadyProcessed =
                        await processedEventRepository.ExistsAsync(
                            paymentEvent.EventId,
                            stoppingToken);

                    if (alreadyProcessed)
                    {
                        consumer.Commit(result);
                        continue;
                    }

                    // Get Order
                    var order =
                        await orderRepository.GetByIdAsync(
                            paymentEvent.OrderId,
                            stoppingToken);

                    if (order is null)
                    {
                        throw new InvalidOperationException(
                            $"Order {paymentEvent.OrderId} was not found.");
                    }

                    var saga =
                        await orderSagaRepository.GetByOrderIdAsync(
                            paymentEvent.OrderId,
                            stoppingToken);

                    if (saga is null)
                    {
                        throw new InvalidOperationException(
                            $"Saga for order {paymentEvent.OrderId} was not found.");
                    }

                    if (paymentEvent.Status == PaymentStatus.Paid)
                    {
                        var completeResult = order.MarkAsPaid();

                        //if (completeResult.IsFailure)
                        //{
                        //    throw new InvalidOperationException(completeResult.Error.Message);
                        //}

                        var cart =
                            await cartRepository.GetByCustomerIdAsync(
                                order.CustomerId,
                                stoppingToken);

                        if (cart is null)
                        {
                            throw new InvalidOperationException("Cart was not found.");
                        }

                        cart.Clear();
                        saga.Complete();

                        await processedEventRepository.AddAsync(
                            new ProcessedEvent(paymentEvent.EventId),
                            stoppingToken);

                        await orderRepository.CompleteAsync(stoppingToken);

                        consumer.Commit(result);
                        continue;
                    }

                    if (paymentEvent.Status == PaymentStatus.Failed)
                    {
                        saga.SetStatus(OrderSagaStatus.Compensating);

                        var releaseStockEvent =
                            new ReleaseStockEvent(
                                Guid.NewGuid(),
                                order.Id,
                                order.Items
                                    .Select(item =>
                                        new OrderItemEvent(
                                            item.ProductId,
                                            item.Quantity))
                                    .ToList());

                        var payload = JsonSerializer.Serialize(releaseStockEvent);

                        var outboxMessage =
                            new OutboxMessage(
                                releaseStockEvent.EventId,
                                "release-stock",
                                order.Id.ToString(),
                                nameof(ReleaseStockEvent),
                                payload);

                        await outboxRepository.AddAsync(outboxMessage);

                        await processedEventRepository.AddAsync(
                            new ProcessedEvent(paymentEvent.EventId),
                            stoppingToken);

                        await orderRepository.CompleteAsync(stoppingToken);

                        consumer.Commit(result);
                        continue;
                    }

                    // Unknown status
                    throw new InvalidOperationException(
                        $"Unsupported payment status: {paymentEvent.Status}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Kafka consumer error: {ex}");

                    // تأخير بسيط عشان ما نعملش loop سريع على نفس الرسالة
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }

            consumer.Close();
        }

    }
}
