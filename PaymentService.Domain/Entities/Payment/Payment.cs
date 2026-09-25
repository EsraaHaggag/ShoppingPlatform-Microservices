using BuildingBlocks.Common;
using PaymentService.Domain.Entities.Payment.Errors;
using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Entities.Payment
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string? ProviderPaymentId { get; private set; }
        public string? CheckoutUrl { get; private set; }
        public DateTime? CheckoutExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Payment() { }

        private Payment(Guid orderId, Guid customerId, decimal amount)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            CustomerId = customerId;
            Amount = amount;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<Payment> Create(
            Guid orderId,
            Guid customerId,
            decimal amount)
        {
            if (amount <= 0)
                return Result<Payment>.Failure(
                  PaymentErrors.InvalidAmount);

            return Result<Payment>.Success(
                new Payment(orderId, customerId,
                    amount));
        }

        public Result SetProviderPayment(
            string providerPaymentId,
            string checkoutUrl,
            DateTime? checkoutExpiresAt = null)
        {
            if (string.IsNullOrWhiteSpace(providerPaymentId))
                return Result.Failure(PaymentErrors.InvalidProviderPaymentId);

            if (string.IsNullOrWhiteSpace(checkoutUrl))
                return Result.Failure(PaymentErrors.InvalidCheckoutUrl);

            ProviderPaymentId = providerPaymentId;
            CheckoutUrl = checkoutUrl;
            CheckoutExpiresAt = checkoutExpiresAt;

            return Result.Success();
        }

        public Result MarkAsSucceeded()
        {
            if (Status == PaymentStatus.Paid)
                return Result.Failure(
                    PaymentErrors.AlreadyPaid);

            if (Status != PaymentStatus.Pending)
                return Result.Failure(PaymentErrors.InvalidStatus);
            Status = PaymentStatus.Paid;
            return Result.Success();
        }

        public Result MarkAsFailed()
        {
            if (Status != PaymentStatus.Pending)
                return Result.Failure(PaymentErrors.InvalidStatus);

            Status = PaymentStatus.Failed;
            return Result.Success();
        }

        public Result Cancel()
        {
            if (Status != PaymentStatus.Pending)
                return Result.Failure(PaymentErrors.InvalidStatus);

            Status = PaymentStatus.Cancelled;
            return Result.Success();
        }
    }
}
