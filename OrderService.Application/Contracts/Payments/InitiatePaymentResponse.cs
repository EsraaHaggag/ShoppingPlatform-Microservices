namespace OrderService.Application.Contracts.Payments
{
    public record InitiatePaymentResponse(
    Guid PaymentId,
    string CheckoutUrl);
}
