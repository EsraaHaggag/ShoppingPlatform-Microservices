using BuildingBlocks.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System.Text.Json;
namespace ProductService.Infrastructure.Messaging
{
    public class OrderPaidConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public OrderPaidConsumer(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(
    CancellationToken stoppingToken)
        {
            Console.WriteLine("🔥 Order Paid Consumer STARTED");
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],

                GroupId = "product-service",

                AutoOffsetReset = AutoOffsetReset.Earliest,

                EnableAutoCommit = false
            };

            using var consumer =
                new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe("order-paid");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    var orderEvent = JsonSerializer.Deserialize<OrderPaidEvent>(result.Message.Value);
                    if (orderEvent is null)
                        continue;
                    using var scope = _scopeFactory.CreateScope();

                    var productRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IProductRepository>();
                    var processedEventRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IProcessedEventRepository>();
                    var alreadyProcessed = await processedEventRepository.ExistsAsync(orderEvent.EventId,
                            stoppingToken);

                    if (alreadyProcessed)
                    {
                        consumer.Commit(result);
                        continue;
                    }
                    var productIds = orderEvent.Items
                        .Select(x => x.ProductId)
                        .ToList();

                    var products = await productRepository.GetByIdsAsync(
                        productIds, stoppingToken);

                    if (products.Count != productIds.Count)
                    {
                        throw new InvalidOperationException(
                            "One or more products were not found.");
                    }
                    var quantities = orderEvent.Items
                    .ToDictionary(x => x.ProductId, x => x.Quantity);

                    foreach (var product in products)
                    {
                        var quantity = quantities[product.Id];

                        var decreaseResult = product.DecreaseStock(quantity);

                        if (decreaseResult.IsFailure)
                        {
                            throw new InvalidOperationException(decreaseResult.Error.Message);
                        }
                    }
                    await processedEventRepository.AddAsync(new ProcessedEvent(orderEvent.EventId), stoppingToken);
                    await productRepository.CompleteAsync(stoppingToken);

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
