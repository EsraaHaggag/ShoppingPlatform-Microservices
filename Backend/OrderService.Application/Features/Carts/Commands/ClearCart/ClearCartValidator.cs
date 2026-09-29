using FluentValidation;

namespace OrderService.Application.Features.Carts.Commands.ClearCart
{
    public class ClearCartValidator
    : AbstractValidator<ClearCartCommand>
    {
        public ClearCartValidator()
        {


        }
    }
}
