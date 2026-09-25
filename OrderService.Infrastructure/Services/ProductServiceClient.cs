using BuildingBlocks.Common;
using OrderService.Application.Features.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Carts.Errors;
using System.Net;
using System.Net.Http.Json;

namespace OrderService.Infrastructure.Services
{
    public class ProductServiceClient : IProductServiceClient
    {
        private readonly HttpClient _httpClient;

        public ProductServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<ProductInfoDto>> GetProductAsync(Guid productId,
        CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(
                $"api/products/{productId}",
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Result<ProductInfoDto>.Failure(
                    CartErrors.ProductNotFound);
            }

            if (!response.IsSuccessStatusCode)
            {
                return Result<ProductInfoDto>.Failure(
                   CartErrors.ServiceUnavailable);
            }

            var product =
                await response.Content.ReadFromJsonAsync<ProductInfoDto>(
                    cancellationToken);

            if (product is null)
            {
                return Result<ProductInfoDto>.Failure(CartErrors.InvalidResponse);
            }

            return Result<ProductInfoDto>.Success(product);
        }

        public async Task<Result<IReadOnlyList<ProductInfoDto>>> GetProductsAsync(
          IReadOnlyCollection<Guid> productIds, CancellationToken cancellationToken)
        {
            var request = new
            {
                ProductIds = productIds
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/products/batch",
                request,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return Result<IReadOnlyList<ProductInfoDto>>.Failure(CartErrors.ServiceUnavailable);
            }

            var products = await response.Content.ReadFromJsonAsync<IReadOnlyList<ProductInfoDto>>(cancellationToken);

            if (products is null)
            {
                return Result<IReadOnlyList<ProductInfoDto>>.Failure(CartErrors.InvalidResponse);
            }
            return Result<IReadOnlyList<ProductInfoDto>>.Success(products);
        }
    }
}
