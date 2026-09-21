using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities.Products.Errors;


namespace ProductService.Application.Features.Products.Commands.Deactivate
{
    public class DeactivateHandler
    : IRequestHandler<DeactivateCommand, Result>
    {
        private readonly IProductRepository _productRepository;

        public DeactivateHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result> Handle(
            DeactivateCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                return Result.Failure(ProductErrors.ProductNotFound);
            var result = product.Deactivate();
            if (result.IsFailure)
                return Result.Failure(result.Error);

            await _productRepository.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
