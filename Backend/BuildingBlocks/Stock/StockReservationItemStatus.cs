namespace BuildingBlocks.Stock
{
    public record StockReservationItemStatus(
    Guid ProductId,
    int Requested,
    int Available,
    decimal? RequestedPrice,
    decimal? CurrentPrice,
    StockAvailabilityStatus Status);
}
