using FluentValidation;

namespace ProductService.Application.Features.Products.Commands.Deactivate
{
    public class DeactivateValidator
    : AbstractValidator<DeactivateCommand>
    {
        public DeactivateValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty();

        }
    }
}

