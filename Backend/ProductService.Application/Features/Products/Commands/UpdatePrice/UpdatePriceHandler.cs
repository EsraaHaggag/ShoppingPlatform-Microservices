using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities.Products.Errors;

namespace ProductService.Application.Features.Products.Commands.UpdatePrice
{
    public class UpdatePriceHandler
    : IRequestHandler<UpdatePriceCommand, Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCacheService _productCacheService;

        public UpdatePriceHandler(
            IProductRepository productRepository, IProductCacheService productCacheService)
        {
            _productRepository = productRepository;
            _productCacheService = productCacheService;
        }

        public async Task<Result> Handle(
            UpdatePriceCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                return Result.Failure(ProductErrors.ProductNotFound);
            var result = product.UpdatePrice(request.Price);
            if (result.IsFailure)
                return Result.Failure(result.Error);

            await _productRepository.CompleteAsync(cancellationToken);
            await _productCacheService.RemoveAsync(
                product.Id,
                cancellationToken);
            return Result.Success();
        }
    }
}
