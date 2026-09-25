using AutoMapper;

namespace OrderService.Application.Mapping.OrderMapping
{
    public partial class OrderProfile : Profile
    {
        public OrderProfile()
        {
            GetOrderByIdMapping();
        }
    }
}
