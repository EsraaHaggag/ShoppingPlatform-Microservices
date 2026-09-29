namespace BuildingBlocks.Events
{
    public record OrderPaidEvent(Guid EventId, Guid OrderId,
    IReadOnlyCollection<OrderItemEvent> Items);

    public record OrderItemEvent(Guid ProductId,
        int Quantity);
}
