using BuildingBlocks.Interfaces;
using PaymentService.Domain.Entities.Payment;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentRepository : IGenericRepositoryAsync<Payment>
    {
        public Task<Payment?> GetLatestByOrderIdAsync(Guid orderId, Guid customerId,
        CancellationToken cancellationToken);

        Task<Payment?> GetByOrderIdAsync(
         Guid orderId, CancellationToken cancellationToken);
    }
}
