using FluentValidation;

namespace PaymentService.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentValidator
    : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentValidator()
        {
            RuleFor(x => x.OrderId)
               .NotEmpty();
            RuleFor(x => x.CustomerId)
                .NotEmpty();
            RuleFor(x => x.Amount)
                .GreaterThan(0);
        }
    }
}
