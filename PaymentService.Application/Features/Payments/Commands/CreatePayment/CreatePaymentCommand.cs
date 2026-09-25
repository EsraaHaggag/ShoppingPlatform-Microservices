using BuildingBlocks.Common;
using MediatR;

namespace PaymentService.Application.Features.Payments.Commands.CreatePayment
{
    public record CreatePaymentCommand(
    Guid OrderId, Guid CustomerId,
    decimal Amount)
    : IRequest<Result<CreatePaymentResponse>>;
}
