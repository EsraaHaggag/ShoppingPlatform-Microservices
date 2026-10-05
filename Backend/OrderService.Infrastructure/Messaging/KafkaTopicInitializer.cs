//using Confluent.Kafka;
//using Confluent.Kafka.Admin;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;

//namespace OrderService.Infrastructure.Messaging;

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

//        var topics = new[]
//        {
//            new TopicSpecification
//            {
//                Name = "payment-status-changed",
//                NumPartitions = 1,
//                ReplicationFactor = 1
//            },
//            new TopicSpecification
//            {
//                Name = "release-stock",
//                NumPartitions = 1,
//                ReplicationFactor = 1
//            }
//        };
//        var metadata = adminClient.GetMetadata(
//    "payment-status-changed",
//    TimeSpan.FromSeconds(10));

//        if (metadata.Topics.Any(t =>
//            t.Topic == "payment-status-changed" &&
//            t.Error.Code == ErrorCode.NoError))
//        {
//            Console.WriteLine(
//                "✅ payment-status-changed already exists.");
//        }
//        else
//        {
//            try
//            {
//                await adminClient.CreateTopicsAsync(
//                    topics,
//                    new CreateTopicsOptions
//                    {
//                        RequestTimeout = TimeSpan.FromSeconds(10)
//                    });

//                Console.WriteLine("✅ Kafka topics created successfully.");
//            }
//            catch (CreateTopicsException ex)
//            {
//                foreach (var result in ex.Results)
//                {
//                    if (result.Error.Code == ErrorCode.TopicAlreadyExists)
//                    {
//                        Console.WriteLine(
//                            $"ℹ️ Kafka topic already exists: {result.Topic}");
//                    }
//                    else
//                    {
//                        throw;
//                    }
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