using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Interfaces
{
    public interface IProductCacheService
    {
        Task<ProductDTO?> GetAsync(
            Guid productId,
            CancellationToken cancellationToken);

        Task SetAsync(
            ProductDTO product,
            CancellationToken cancellationToken);
        Task<IReadOnlyList<ProductDTO>> GetManyAsync(
        IReadOnlyCollection<Guid> productIds,
        CancellationToken cancellationToken);
        Task RemoveAsync(
            Guid productId,
            CancellationToken cancellationToken);
    }
}
