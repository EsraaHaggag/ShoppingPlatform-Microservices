namespace BuildingBlocks.Events
{
    public record ReleaseStockEvent(
    Guid EventId,
    Guid OrderId,
    IReadOnlyList<OrderItemEvent> Items);
}
