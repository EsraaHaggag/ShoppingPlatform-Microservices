namespace BuildingBlocks.Stock
{
    public record ReserveStockRequest(
    IReadOnlyList<ReserveStockItem> Items);
}
