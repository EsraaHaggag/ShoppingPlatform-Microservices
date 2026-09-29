namespace BuildingBlocks.Stock
{
    public record ReserveStockItem(
    Guid ProductId,
    int RequestedQuantity,
    decimal ExpectedPrice);
}
