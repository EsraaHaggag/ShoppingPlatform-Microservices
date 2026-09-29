namespace OrderService.Application.Contracts.Payments
{
    public record InitiatePaymentRequest(
    Guid OrderId, Guid CustomerId,
    decimal Amount);
}
