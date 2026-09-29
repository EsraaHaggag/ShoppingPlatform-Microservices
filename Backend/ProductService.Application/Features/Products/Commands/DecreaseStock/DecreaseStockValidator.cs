

using FluentValidation;

namespace ProductService.Application.Features.Products.Commands.DecreaseStock
{
    public class DecreaseStockValidator
    : AbstractValidator<DecreaseStockCommand>
    {
        public DecreaseStockValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty();
            RuleFor(x => x.quantity)
                .GreaterThan(0);
        }
    }
}
