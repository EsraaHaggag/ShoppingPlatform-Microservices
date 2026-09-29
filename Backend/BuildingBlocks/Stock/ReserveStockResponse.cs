namespace BuildingBlocks.Stock
{
    public record ReserveStockResponse(
    bool Success,
    string? Message,
    IReadOnlyList<StockReservationItemStatus> ItemsStatus);
}
