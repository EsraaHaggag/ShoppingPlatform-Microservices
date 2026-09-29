using BuildingBlocks.Repositories;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities.Payment;
using PaymentService.Infrastructure.Persistence;

namespace PaymentService.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepositoryAsync<Payment>, IPaymentRepository
    {
        private readonly DbSet<Payment> _payments;
        public PaymentRepository(PaymentDbContext dbContext) : base(dbContext)
        {
            _payments = dbContext.Payments;
        }

        public async Task<Payment?> GetLatestByOrderIdAsync(Guid orderId,
          Guid customerId,
          CancellationToken cancellationToken)
        {
            return await _payments
                .Where(x =>
                    x.OrderId == orderId &&
                    x.CustomerId == customerId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<Payment?> GetByOrderIdAsync(
        Guid orderId, CancellationToken cancellationToken)
        {
            return await _payments
                .Where(x => x.OrderId == orderId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
