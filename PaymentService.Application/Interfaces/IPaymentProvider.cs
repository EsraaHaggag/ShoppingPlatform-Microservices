using PaymentService.Application.Contracts.Payments;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentProvider
    {
        Task<PaymentProviderResult> CreatePaymentAsync(
        PaymentProviderRequest request,
        CancellationToken cancellationToken);


    }
}
