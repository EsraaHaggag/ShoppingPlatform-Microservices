namespace PaymentService.Application.Contracts.Payments
{
    public record PaymentProviderResult(
    bool IsSuccess,
    string? ProviderPaymentId,
    string? CheckoutUrl,
    string? ErrorMessage);
}
