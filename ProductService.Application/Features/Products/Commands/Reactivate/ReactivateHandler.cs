using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities.Products.Errors;

namespace ProductService.Application.Features.Products.Commands.Reactivate
{
    public class ReactivateHandler
    : IRequestHandler<ReactivateCommand, Result>
    {
        private readonly IProductRepository _productRepository;

        public ReactivateHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result> Handle(
            ReactivateCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                return Result.Failure(ProductErrors.ProductNotFound);
            var result = product.Reactivate();
            if (result.IsFailure)
                return Result.Failure(result.Error);

            await _productRepository.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}

