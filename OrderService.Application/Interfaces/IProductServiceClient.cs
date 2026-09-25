using BuildingBlocks.Common;
using OrderService.Application.Features.DTOs;

namespace OrderService.Application.Interfaces
{
    public interface IProductServiceClient
    {
        Task<Result<ProductInfoDto>> GetProductAsync(
         Guid productId,
         CancellationToken cancellationToken);

        Task<Result<IReadOnlyList<ProductInfoDto>>> GetProductsAsync(
        IReadOnlyCollection<Guid> productIds,
        CancellationToken cancellationToken);
    }
}
