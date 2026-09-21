
using FluentValidation;

namespace ProductService.Application.Features.Products.Commands.IncreaseStock
{
    public class IncreaseStockValidator
    : AbstractValidator<IncreaseStockCommand>
    {
        public IncreaseStockValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty();
            RuleFor(x => x.quantity)
                .GreaterThan(0);
        }
    }
}
