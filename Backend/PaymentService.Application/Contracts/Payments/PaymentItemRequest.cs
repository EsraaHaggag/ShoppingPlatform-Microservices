namespace PaymentService.Application.Contracts.Payments
{
    public record PaymentItemRequest(
    string Name,
    decimal Amount,
    int Quantity,
    string Description);
}
