namespace BuildingBlocks.Events
{

    public record ReserveStockEvent(
        Guid EventId,
        Guid OrderId,
        IReadOnlyList<ReserveStockItem> Items);

    public record ReserveStockItem(
        Guid ProductId,
        int Quantity);


}
