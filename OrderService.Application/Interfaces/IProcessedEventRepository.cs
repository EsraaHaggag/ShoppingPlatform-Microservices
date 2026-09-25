using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces
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
