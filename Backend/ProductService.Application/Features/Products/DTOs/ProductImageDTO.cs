namespace ProductService.Application.Features.Products.DTOs
{
    public class ProductImageDTO
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPrimary { get; set; }
    }
}
