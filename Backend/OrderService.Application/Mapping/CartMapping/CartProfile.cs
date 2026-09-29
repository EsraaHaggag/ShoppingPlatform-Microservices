using AutoMapper;

namespace OrderService.Application.Mapping.CartMapping
{
    public partial class CartProfile : Profile
    {
        public CartProfile()
        {
            GetCartItemsMapping();
        }

    }
}
