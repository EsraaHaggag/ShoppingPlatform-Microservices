
using FluentValidation;

namespace ProductService.Application.Features.Products.Commands.Reactivate
{
    public class ReactivateValidator
    : AbstractValidator<ReactivateCommand>
    {
        public ReactivateValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty();

        }
    }
}
