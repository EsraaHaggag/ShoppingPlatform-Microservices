using BuildingBlocks.Common;
using BuildingBlocks.Stock;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;

namespace ProductService.Application.Features.Products.Commands.ReserveStock
{
    public class ReserveStockHandler
    : IRequestHandler<ReserveStockCommand,
    Result<ReserveStockResponse>>
    {
        private readonly IProductRepository _productRepository;

        public ReserveStockHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<Result<ReserveStockResponse>> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
        {
            var productIds = request.Items
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);
            var productDictionary = products.ToDictionary(x => x.Id);

            var statuses = new List<StockReservationItemStatus>(request.Items.Count);

            foreach (var item in request.Items)
            {
                if (!productDictionary.TryGetValue(item.ProductId, out var product))
                {
                    statuses.Add(new StockReservationItemStatus(
                        item.ProductId,
                        item.RequestedQuantity,
                        0,
                        item.ExpectedPrice,
                        null, StockAvailabilityStatus.ProductNotFound));

                    continue;
                }
                StockAvailabilityStatus itemStatus;

                if (product.Price != item.ExpectedPrice)
                {
                    itemStatus = StockAvailabilityStatus.PriceChanged;
                }
                else if (product.StockQuantity == 0)
                {
                    itemStatus = StockAvailabilityStatus.OutOfStock;
                }
                else if (product.StockQuantity < item.RequestedQuantity)
                {
                    itemStatus = StockAvailabilityStatus.Partial;
                }
                else
                {
                    itemStatus = StockAvailabilityStatus.Available;
                }

                statuses.Add(new StockReservationItemStatus(
                    product.Id,
                    item.RequestedQuantity,
                    product.StockQuantity,
                    item.ExpectedPrice,
                    product.Price,
                    itemStatus));
            }

            var hasErrors = statuses.Any(x => x.Status != StockAvailabilityStatus.Available);

            if (hasErrors)
            {
                return Result<ReserveStockResponse>.Success(
                    new ReserveStockResponse(
                        Success: false,
                        Message: "Some products are unavailable or have changes.",
                        ItemsStatus: statuses));
            }

            foreach (var item in request.Items)
            {
                var product = productDictionary[item.ProductId];

                var decreaseResult = product.DecreaseStock(item.RequestedQuantity);

                if (decreaseResult.IsFailure)
                {
                    return Result<ReserveStockResponse>.Failure(decreaseResult.Error);
                }
            }
            try
            {
                await _productRepository.CompleteAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<ReserveStockResponse>.Failure(
                  new Error("Product.ConcurrencyConflict", "Sorry, the product stock has just changed. Please try again."));
            }

            return Result<ReserveStockResponse>.Success(
                new ReserveStockResponse(
                    Success: true,
                    Message: null,
                    ItemsStatus: statuses));
        }
    }
}
