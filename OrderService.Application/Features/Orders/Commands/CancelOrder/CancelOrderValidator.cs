using FluentValidation;

namespace OrderService.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderValidator
    : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderValidator()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty();
        }
    }
}
