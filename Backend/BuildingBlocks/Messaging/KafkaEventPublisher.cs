using BuildingBlocks.Interfaces;
using Confluent.Kafka;

namespace BuildingBlocks.Messaging
{
    public class KafkaEventPublisher : IEventPublisher
    {

        private readonly IProducer<string, string> _producer;

        public KafkaEventPublisher(
            IProducer<string, string> producer)
        {
            _producer = producer;
        }

        public async Task PublishAsync(string topic,
           string key, string payload, CancellationToken cancellationToken)
        {
            var result = await _producer.ProduceAsync(
                topic,
                new Message<string, string>
                {
                    Key = key,
                    Value = payload
                },
                cancellationToken);

            if (result.Status != PersistenceStatus.Persisted)
            {
                throw new InvalidOperationException(
                    $"Kafka message not persisted. Status: {result.Status}");
            }
        }
    }
}
