using BuildingBlocks.Events;
using BuildingBlocks.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Entities.Carts;
using System.Text.Json;
namespace OrderService.Infrastructure.Messaging
{
    public class PaymentStatusChangeConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IConfiguration _configuration;

        public PaymentStatusChangeConsumer(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            Console.WriteLine("🔥 PaymentStatusChangeConsumer STARTED");
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],

                GroupId = "order-service",

                AutoOffsetReset = AutoOffsetReset.Earliest,

                EnableAutoCommit = false
            };

            using var consumer =
                new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe("payment-status-changed");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    var paymentEvent = JsonSerializer.Deserialize<PaymentStatusChangedEvent>(result.Message.Value);

                    if (paymentEvent is null)
                        continue;

                    using var scope = _scopeFactory.CreateScope();

                    var orderRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IOrderRepository>();
                    var cartRepository =
                        scope.ServiceProvider
                            .GetRequiredService<ICartRepository>();

                    var outboxRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IOutboxRepository>();
                    var processedEventRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IProcessedEventRepository>();
                    var alreadyProcessed = await processedEventRepository.ExistsAsync(paymentEvent.EventId,
                            stoppingToken);

                    if (alreadyProcessed)
                    {
                        consumer.Commit(result);
                        continue;
                    }
                    var order = await orderRepository.GetByIdAsync(
                        paymentEvent.OrderId,
                        stoppingToken);

                    if (order is null)
                    {
                        throw new InvalidOperationException(
                            $"Order {paymentEvent.OrderId} was not found.");
                    }
                    var CompleteResult = order.MarkAsPaid();

                    if (CompleteResult.IsFailure)
                    {
                        throw new InvalidOperationException(CompleteResult.Error.Message);
                    }

                    var cart = await cartRepository.GetByCustomerIdAsync(order.CustomerId, stoppingToken);

                    if (cart is null)
                    {
                        throw new InvalidOperationException(
                            $"Cart was not found.");
                    }
                    cart.Clear();


                    var orderPaidEvent = new OrderPaidEvent(
                        Guid.NewGuid(),
                        order.Id,
                        order.Items.Select(item =>
                            new OrderItemEvent(
                                item.ProductId,
                                item.Quantity))
                        .ToList());

                    var payload = JsonSerializer.Serialize(orderPaidEvent);

                    var outboxMessage = new OutboxMessage(
                        orderPaidEvent.EventId,
                        "order-paid",
                        order.Id.ToString(),
                        nameof(OrderPaidEvent),
                        payload);

                    await outboxRepository.AddAsync(
                        outboxMessage);
                    await processedEventRepository.AddAsync(new ProcessedEvent(paymentEvent.EventId), stoppingToken);

                    await orderRepository.CompleteAsync(stoppingToken);

                    consumer.Commit(result);

                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Kafka consumer error: {ex.Message}");
                }
            }
            consumer.Close();
        }
    }
}
