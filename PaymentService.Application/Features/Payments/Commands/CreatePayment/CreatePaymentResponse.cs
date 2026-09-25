namespace PaymentService.Application.Features.Payments.Commands.CreatePayment
{
    public record CreatePaymentResponse(
    Guid PaymentId,
    string CheckoutUrl);
}
