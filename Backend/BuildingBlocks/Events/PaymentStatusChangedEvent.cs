namespace BuildingBlocks.Events
{
    public record PaymentStatusChangedEvent(Guid EventId, Guid PaymentId, Guid OrderId, decimal Amount, PaymentStatus Status);

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Cancelled
    }
}
