using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces
{
    public interface IProcessedEventRepository
    {
        Task<bool> ExistsAsync(
            Guid eventId,
            CancellationToken cancellationToken);
        Task AddAsync(
            ProcessedEvent processedEvent,
            CancellationToken cancellationToken);
    }
}
