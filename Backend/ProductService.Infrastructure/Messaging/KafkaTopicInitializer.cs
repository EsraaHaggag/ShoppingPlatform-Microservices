//using Confluent.Kafka;
//using Confluent.Kafka.Admin;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;

//namespace ProductService.Infrastructure.Messaging;

//public class KafkaTopicInitializer : IHostedService
//{
//    private readonly IConfiguration _configuration;

//    public KafkaTopicInitializer(IConfiguration configuration)
//    {
//        _configuration = configuration;
//    }

//    public async Task StartAsync(CancellationToken cancellationToken)
//    {
//        var kafkaConnection =
//            _configuration.GetConnectionString("kafka");

//        if (string.IsNullOrWhiteSpace(kafkaConnection))
//            throw new InvalidOperationException(
//                "Kafka connection string was not found.");

//        var config = new AdminClientConfig
//        {
//            BootstrapServers = kafkaConnection
//        };

//        using var adminClient =
//            new AdminClientBuilder(config).Build();

//        try
//        {
//            await adminClient.CreateTopicsAsync(
//                new[]
//                {
//                    new TopicSpecification
//                    {
//                        Name = "release-stock",
//                        NumPartitions = 1,
//                        ReplicationFactor = 1
//                    }
//                },
//                new CreateTopicsOptions
//                {
//                    RequestTimeout = TimeSpan.FromSeconds(10)
//                });

//            Console.WriteLine(
//                "✅ release-stock topic created.");
//        }
//        catch (CreateTopicsException ex)
//        {
//            foreach (var result in ex.Results)
//            {
//                if (result.Error.Code ==
//                    ErrorCode.TopicAlreadyExists)
//                {
//                    Console.WriteLine(
//                        "ℹ️ release-stock already exists.");
//                }
//                else
//                {
//                    throw;
//                }
//            }
//        }
//    }

//    public Task StopAsync(
//        CancellationToken cancellationToken)
//    {
//        return Task.CompletedTask;
//    }
//}