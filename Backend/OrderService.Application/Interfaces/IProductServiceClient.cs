using BuildingBlocks.Common;
using BuildingBlocks.Stock;
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

        Task<Result<ReserveStockResponse>> ReserveStockAsync(
            IReadOnlyCollection<ReserveStockItem> items,
            CancellationToken cancellationToken);
    }
}
