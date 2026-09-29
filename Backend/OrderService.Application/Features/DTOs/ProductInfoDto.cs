namespace OrderService.Application.Features.DTOs
{
    public record ProductInfoDto(
        Guid Id,
        string Name,
        decimal Price,
        int StockQuantity);
}
