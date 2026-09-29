using FluentValidation;

namespace OrderService.Application.Features.Carts.Commands.RemoveItem
{
    public class RemoveCartItemValidator
    : AbstractValidator<RemoveCartItemCommand>
    {
        public RemoveCartItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();

        }
    }
}
