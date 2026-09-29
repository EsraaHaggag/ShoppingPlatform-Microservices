using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Infrastructure.Repositories
{
    public class ProcessedEventRepository : IProcessedEventRepository
    {
        private readonly ProductDbContext _dbContext;

        public ProcessedEventRepository(ProductDbContext dbContext)
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
