namespace OrderService.Application.Features.DTOs
{
    public class CartItemDto
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string? ProductImageUrl { get; set; } = null;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
