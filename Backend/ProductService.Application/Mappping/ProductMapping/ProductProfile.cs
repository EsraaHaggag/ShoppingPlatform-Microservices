using AutoMapper;

namespace ProductService.Application.Mappping.ProductMapping
{
    public partial class ProductProfile : Profile
    {
        public ProductProfile()
        {
            GetAllProductsMapping();
        }
    }
}
