namespace ProductService.Application.Features.Products.DTOs
{
    public record ProductInfoDto(
        Guid Id,
        string Name,
        decimal Price,
        int StockQuantity);
}
