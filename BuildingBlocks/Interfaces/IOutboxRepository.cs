using OrderService.Domain.Entities.Carts;

namespace BuildingBlocks.Interfaces
{
    public interface IOutboxRepository : IGenericRepositoryAsync<OutboxMessage>
    {
        Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(
            int batchSize,
            CancellationToken cancellationToken);

    }
}
