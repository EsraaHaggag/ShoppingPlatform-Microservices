using FluentValidation;

namespace OrderService.Application.Features.Carts.Commands.AddCartItem
{
    public class AddCartItemValidator
    : AbstractValidator<AddCartItemCommand>
    {
        public AddCartItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();

            RuleFor(x => x.CustomerId)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .NotEmpty();
        }
    }
}