using FluentValidation;


namespace ProductService.Application.Features.Products.Commands.UpdatePrice
{
    public class UpdatePriceValidator
    : AbstractValidator<UpdatePriceCommand>
    {
        public UpdatePriceValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty();
            RuleFor(x => x.Price)
                .GreaterThan(0);
        }
    }
}
