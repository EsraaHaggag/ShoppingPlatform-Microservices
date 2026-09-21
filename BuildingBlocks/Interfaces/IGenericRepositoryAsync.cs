namespace BuildingBlocks.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task<int> CompleteAsync(CancellationToken cancellationToken);
    }
}
