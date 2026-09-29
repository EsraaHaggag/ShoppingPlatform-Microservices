using BuildingBlocks.Common;
using MediatR;

namespace PaymentService.Application.Features.Payments.Commands.ProcessPaymobWebhook
{
    public record ProcessPaymobWebhookCommand(
    PaymobWebhookRequest Request
) : IRequest<Result>;
}
