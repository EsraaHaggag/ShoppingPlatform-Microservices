using BuildingBlocks.Interfaces;
using BuildingBlocks.Repositories;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities.Carts;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories
{
    public class OutboxRepository
    : GenericRepositoryAsync<OutboxMessage>,
      IOutboxRepository
    {
        private readonly DbSet<OutboxMessage> _outboxMessages;
        public OutboxRepository(OrderDbContext dbContext)
            : base(dbContext)
        {
            _outboxMessages = dbContext.OutboxMessages;
        }

        public async Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(
        int batchSize,
        CancellationToken cancellationToken)
        {
            return await _outboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
                .Take(batchSize)
                .ToListAsync(cancellationToken);
        }

    }
}
