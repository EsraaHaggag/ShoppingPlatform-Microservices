using FluentValidation;

namespace OrderService.Application.Features.Orders.Commands.Checkout
{
    public class CheckoutValidator
    : AbstractValidator<CheckoutCommand>
    {
        public CheckoutValidator()
        {


        }
    }
}
