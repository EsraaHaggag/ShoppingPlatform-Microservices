using ProductService.Application.Features.Products.DTOs;
using ProductService.Domain.Entities;
using ProductService.Domain.Entities.Products;

namespace ProductService.Application.Mappping.ProductMapping
{
    public partial class ProductProfile
    {
        public void GetAllProductsMapping()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, ProductInfoDto>();
            CreateMap<ProductImage, ProductImageDTO>();

        }
    }
}
