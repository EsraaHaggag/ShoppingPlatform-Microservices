using AutoMapper;
using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Features.Products.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities.Products.Errors;

namespace ProductService.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, Result<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdHandler(
            IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProductDTO>> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (result == null)
                return Result<ProductDTO>.Failure(ProductErrors.ProductNotFound);
            var productDto = _mapper.Map<ProductDTO>(result);
            return Result<ProductDTO>.Success(productDto);
        }
    }
}
