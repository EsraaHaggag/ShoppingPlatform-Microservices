using ProductService.Domain.Entities.Products;

namespace ProductService.Domain.Entities
{
    public class ProductImage
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public string ImageUrl { get; private set; }

        public int DisplayOrder { get; private set; }

        public bool IsPrimary { get; private set; }

        public Product Product { get; private set; }

        private ProductImage()
        {
        }

        public ProductImage(
            Guid productId,
            string imageUrl,
            int displayOrder,
            bool isPrimary)
        {
            ProductId = productId;
            ImageUrl = imageUrl;
            DisplayOrder = displayOrder;
            IsPrimary = isPrimary;
        }
    }
}
