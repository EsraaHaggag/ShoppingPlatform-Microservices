using BuildingBlocks.Interfaces;
using Confluent.Kafka;

namespace BuildingBlocks.Messaging
{
    public class KafkaEventPublisher : IEventPublisher
    {
        //private readonly IProducer<string, string> _producer;

        //public KafkaEventPublisher(
        //    IProducer<string, string> producer)
        //{
        //    _producer = producer;
        //}

        //public async Task PublishAsync<T>(
        //    string topic,
        //    string key,
        //    T message,
        //    CancellationToken cancellationToken)
        //{
        //    var json = JsonSerializer.Serialize(message);

        //    await _producer.ProduceAsync(
        //        topic,
        //        new Message<string, string>
        //        {
        //            Key = key,
        //            Value = json
        //        },
        //        cancellationToken);
        //}

        private readonly IProducer<string, string> _producer;

        public KafkaEventPublisher(
            IProducer<string, string> producer)
        {
            _producer = producer;
        }

        public async Task PublishAsync(
            string topic,
            string key,
            string payload,
            CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(
                topic,
                new Message<string, string>
                {
                    Key = key,
                    Value = payload
                },
                cancellationToken);
        }
    }
}
