using OrderService.Application.Features.DTOs;
using OrderService.Domain.Entities.Carts;

namespace OrderService.Application.Mapping.CartMapping
{
    public partial class CartProfile
    {
        public void GetCartItemsMapping()
        {
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>();
        }
    }
}
