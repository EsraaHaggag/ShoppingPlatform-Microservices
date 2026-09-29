using BuildingBlocks.Common;

namespace PaymentService.Domain.Entities.Payment.Errors
{
    public static class PaymentErrors
    {
        public static readonly Error ProviderFailed =
        new Error(
            "Payment.ProviderFailed",
            "The payment provider failed to process the payment.",
            ErrorType.Failure);

        public static readonly Error AlreadyPaid =
            Error.Conflict(
                "Payment.AlreadyPaid",
                "The order has already been paid.");


        public static readonly Error InvalidOrder =
            Error.Validation(
                "Payment.InvalidOrder",
                "Order is invalid.");

        public static readonly Error InvalidCustomer =
            Error.Validation(
                "Payment.InvalidCustomer",
                "Customer is invalid.");

        public static readonly Error InvalidAmount =
            Error.Validation(
                "Payment.InvalidAmount",
                "Payment amount must be greater than zero.");

        public static readonly Error InvalidProviderPaymentId =
            Error.Validation(
            "Payment.InvalidProviderPaymentId",
                "Provider payment ID is required.");

        public static readonly Error InvalidCheckoutUrl =
            Error.Validation(
                "Payment.InvalidCheckoutUrl",
                "Checkout URL is required.");

        public static readonly Error InvalidStatus =
            Error.Conflict(
                "Payment.InvalidStatus",
                "The payment cannot be modified from its current status.");
    }
}
