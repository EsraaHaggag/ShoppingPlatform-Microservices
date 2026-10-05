namespace OrderService.Application.Features.DTOs
{
    public record ProductInfoDto(
        Guid Id,
        string Name,
        decimal Price,
        int StockQuantity,
        List<ProductImageInfoDto> Images);

    public record ProductImageInfoDto(
    Guid Id,
    string ImageUrl,
    int DisplayOrder,
    bool IsPrimary);
}
