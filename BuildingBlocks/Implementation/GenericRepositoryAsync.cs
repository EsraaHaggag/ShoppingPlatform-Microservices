using BuildingBlocks.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Repositories
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        protected readonly DbContext _dbContext;

        public GenericRepositoryAsync(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>()
                .FindAsync(new object[] { id }, cancellationToken);
        }

        public IQueryable<T> GetTableNoTracking()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetTableAsTracking()
        {
            return _dbContext.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public virtual Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task AddRangeAsync(ICollection<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
            return Task.CompletedTask;
        }

        public virtual Task UpdateRangeAsync(ICollection<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return Task.CompletedTask;
        }

        public virtual Task DeleteRangeAsync(ICollection<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public virtual async Task<int> CompleteAsync(
    CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
