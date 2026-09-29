using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories
{
    public class ProcessedEventRepository : IProcessedEventRepository
    {
        private readonly OrderDbContext _dbContext;

        public ProcessedEventRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(
            Guid eventId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.ProcessedEvents
                .AnyAsync(x => x.EventId == eventId, cancellationToken);
        }

        public async Task AddAsync(
            ProcessedEvent processedEvent,
            CancellationToken cancellationToken)
        {
            await _dbContext.ProcessedEvents.AddAsync(
                processedEvent,
                cancellationToken);
        }
    }
}
