namespace BuildingBlocks.Events
{
    public record PaymentStatusChangedEvent(Guid EventId,
    Guid PaymentId, Guid OrderId, decimal Amount);
}
