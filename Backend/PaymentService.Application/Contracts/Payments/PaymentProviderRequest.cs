namespace PaymentService.Application.Contracts.Payments
{
    public record PaymentProviderRequest(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    string Currency);
}
