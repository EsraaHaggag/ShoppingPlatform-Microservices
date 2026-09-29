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
    public class ReleaseStockConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public ReleaseStockConsumer(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            Console.WriteLine("🔥 Release Stock Consumer STARTED");

            var kafkaConnection = _configuration.GetConnectionString("kafka");
            Console.WriteLine($"🔌 Consumer Kafka Connection: {kafkaConnection}");

            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaConnection,
                GroupId = "product-service-release-stock",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            using var consumer =
                new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe("release-stock");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    var releaseStockEvent =
                        JsonSerializer.Deserialize<ReleaseStockEvent>(
                            result.Message.Value);

                    if (releaseStockEvent is null)
                        continue;

                    using var scope =
                        _scopeFactory.CreateScope();

                    var productRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IProductRepository>();

                    var processedEventRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IProcessedEventRepository>();

                    // Idempotency
                    var alreadyProcessed =
                        await processedEventRepository.ExistsAsync(releaseStockEvent.EventId,
                            stoppingToken);

                    if (alreadyProcessed)
                    {
                        consumer.Commit(result);
                        continue;
                    }

                    var productIds = releaseStockEvent.Items
                        .Select(x => x.ProductId)
                        .ToList();

                    var products =
                        await productRepository.GetByIdsAsync(productIds,
                            stoppingToken);

                    var quantities =
                        releaseStockEvent.Items
                            .ToDictionary(
                                x => x.ProductId,
                                x => x.Quantity);

                    foreach (var product in products)
                    {
                        var quantity =
                            quantities[product.Id];

                        product.IncreaseStock(quantity);
                    }

                    await processedEventRepository.AddAsync(
                        new ProcessedEvent(
                            releaseStockEvent.EventId),
                        stoppingToken);

                    await productRepository.CompleteAsync(
                        stoppingToken);

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
