using BuildingBlocks.Common;
using OrderService.Application.Contracts.Payments;

namespace OrderService.Application.Interfaces
{
    public interface IPaymentServiceClient
    {
        Task<Result<InitiatePaymentResponse>> InitiatePaymentAsync(Guid orderId,
          Guid customerId, decimal amount,
        CancellationToken cancellationToken);
    }
}
