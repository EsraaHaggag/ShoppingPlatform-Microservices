using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Common;
using BuildingBlocks.Extensions;
using MediatR;
using ProductService.Application.Features.Products.DTOs;
using ProductService.Application.Interfaces;

namespace ProductService.Application.Features.Products.Queries.GetAllProduct
{
    public class GetAllProductsHandler
    : IRequestHandler<GetAllProductsQuery, Result<PaginatedResult<ProductDTO>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetAllProductsHandler(
            IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<Result<PaginatedResult<ProductDTO>>> Handle(
    GetAllProductsQuery request,
    CancellationToken cancellationToken)
        {
            var query = _productRepository.GetQueryable();

            if (request.MinPrice.HasValue)
                query = query.Where(v => v.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(v => v.Price <= request.MaxPrice.Value);

            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "price":
                        query = request.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(v => v.Price)
                            : query.OrderBy(v => v.Price);
                        break;

                    default:
                        query = query.OrderBy(v => v.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(v => v.Id);
            }

            var result = await query
                .ProjectTo<ProductDTO>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(
                    request.PageNumber,
                    request.PageSize);

            return Result<PaginatedResult<ProductDTO>>.Success(result);
        }
    }
}

