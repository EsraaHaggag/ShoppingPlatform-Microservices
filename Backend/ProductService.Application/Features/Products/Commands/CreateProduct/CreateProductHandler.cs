using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities.Products;

namespace ProductService.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<Guid>> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var result = Product.Create(
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity);

            if (result.IsFailure)
                return Result<Guid>.Failure(result.Error);

            await _productRepository.AddAsync(result.Value);

            await _productRepository.CompleteAsync(cancellationToken);

            return Result<Guid>.Success(result.Value.Id);
        }
    }
}
