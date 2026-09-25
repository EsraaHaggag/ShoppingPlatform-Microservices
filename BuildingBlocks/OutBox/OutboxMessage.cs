namespace OrderService.Domain.Entities.Carts
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public string Topic { get; private set; } = null!;
        public string Key { get; private set; } = null!;
        public string Type { get; private set; } = null!;
        public string Payload { get; private set; } = null!;
        public DateTime OccurredOnUtc { get; private set; }
        public DateTime? ProcessedOnUtc { get; private set; }
        public string? Error { get; private set; }

        private OutboxMessage()
        {
        }

        public OutboxMessage(
            Guid id,
            string topic,
            string key,
            string type,
            string payload)
        {
            Id = id;
            Topic = topic;
            Key = key;
            Type = type;
            Payload = payload;
            OccurredOnUtc = DateTime.UtcNow;
        }

        public void MarkAsProcessed()
        {
            ProcessedOnUtc = DateTime.UtcNow;
            Error = null;
        }

        public void MarkAsFailed(string error)
        {
            Error = error;
        }
    }
}
