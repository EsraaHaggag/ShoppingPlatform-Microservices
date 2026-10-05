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
        public async Task<Result<PaginatedResult<ProductDTO>>> Handle(GetAllProductsQuery request,
          CancellationToken cancellationToken)
        {
            var query = _productRepository.GetQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search));
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price <= request.MaxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "price":
                        query = request.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(p => p.Price)
                            : query.OrderBy(p => p.Price);
                        break;

                    case "name":
                        query = request.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(p => p.Name)
                            : query.OrderBy(p => p.Name);
                        break;

                    default:
                        query = query.OrderBy(p => p.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            var result = await query
                .ProjectTo<ProductDTO>(
                    _mapper.ConfigurationProvider)
                .ToPaginatedListAsync(
                    request.PageNumber,
                    request.PageSize);

            return Result<PaginatedResult<ProductDTO>>.Success(result);
        }
    }
}

