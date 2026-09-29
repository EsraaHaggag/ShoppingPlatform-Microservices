using BuildingBlocks.Common;
using BuildingBlocks.Events;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Domain.Entities.Carts;
using PaymentService.Application.Interfaces;
using System.Text.Json;

namespace PaymentService.Application.Features.Payments.Commands.ProcessPaymobWebhook
{
    public class ProcessPaymobWebhookHandler
    : IRequestHandler<ProcessPaymobWebhookCommand, Result>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IEventPublisher _eventPublisher;

        public ProcessPaymobWebhookHandler(
            IPaymentRepository paymentRepository, IEventPublisher eventPublisher, IOutboxRepository outboxRepository)
        {
            _paymentRepository = paymentRepository;
            _eventPublisher = eventPublisher;
            _outboxRepository = outboxRepository;
        }
        public async Task<Result> Handle(
    ProcessPaymobWebhookCommand request,
    CancellationToken cancellationToken)
        {
            var webhook = request.Request;

            if (webhook.Type != "TRANSACTION")
                return Result.Success();

            if (!Guid.TryParse(
                webhook.Obj.Order.MerchantOrderId,
                out var paymentId))
            {
                return Result.Failure(
                    new Error(
                        "Payment.InvalidPaymentId",
                        "Invalid payment id in Paymob webhook.",
                        ErrorType.Validation));
            }

            var payment = await _paymentRepository.GetByIdAsync(
                paymentId,
                cancellationToken);

            if (payment is null)
            {
                return Result.Failure(
                    new Error(
                        "Payment.NotFound",
                        "Payment not found.",
                        ErrorType.NotFound));
            }

            if (webhook.Obj.Success)
            {
                var result = payment.MarkAsSucceeded();

                //if (result.IsFailure)
                //    return Result.Failure(result.Error);
            }
            else
            {
                var result = payment.MarkAsFailed();

                if (result.IsFailure)
                    return Result.Failure(result.Error);
            }
            Console.WriteLine($"🔥 Payment Status: {payment.Status}");
            var paymentEvent = new PaymentStatusChangedEvent(
                Guid.NewGuid(),
                payment.Id,
                payment.OrderId,
                payment.Amount,
                payment.Status);

            var payload = JsonSerializer.Serialize(paymentEvent);
            Console.WriteLine("🔥 PAYLOAD BEFORE OUTBOX:");
            Console.WriteLine(payload);
            var outboxMessage = new OutboxMessage(
                paymentEvent.EventId,
                "payment-status-changed",
                payment.OrderId.ToString(),
                nameof(PaymentStatusChangedEvent),
                payload);

            await _outboxRepository.AddAsync(
                outboxMessage);

            await _paymentRepository.CompleteAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}
