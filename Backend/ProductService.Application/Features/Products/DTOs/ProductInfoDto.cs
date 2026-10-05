namespace ProductService.Application.Features.Products.DTOs
{
    public record ProductInfoDto(
    Guid Id,
    string Name,
    decimal Price,
    int StockQuantity,
    List<ProductImageDTO> Images)
    {
        public string? Image => Images?
            .FirstOrDefault(img => img.IsPrimary)?.ImageUrl
            ?? Images?.FirstOrDefault()?.ImageUrl;
    }
}
