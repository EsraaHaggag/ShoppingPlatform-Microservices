using ProductService.Application.Features.Products.DTOs;
using ProductService.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace ProductService.Infrastructure.Caching
{
    public class ProductCacheService : IProductCacheService
    {
        private readonly IDatabase _database;

        public ProductCacheService(
            IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<ProductDTO?> GetAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var key = $"product:{productId}";

            var value = await _database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<ProductDTO>(
                value.ToString());
        }

        public async Task<IReadOnlyList<ProductDTO>> GetManyAsync(IReadOnlyCollection<Guid> productIds, CancellationToken cancellationToken)
        {
            if (productIds.Count == 0)
                return [];

            var keys = productIds
                .Select(id => (RedisKey)$"product:{id}").ToArray();

            var values = await _database.StringGetAsync(keys);

            var products = new List<ProductDTO>();

            foreach (var value in values)
            {
                if (value.IsNullOrEmpty)
                    continue;

                var product = JsonSerializer.Deserialize<ProductDTO>(value.ToString());

                if (product is not null)
                    products.Add(product);
            }

            return products;
        }

        public async Task SetAsync(
            ProductDTO product,
            CancellationToken cancellationToken)
        {
            var key = $"product:{product.Id}";

            var json = JsonSerializer.Serialize(product);

            await _database.StringSetAsync(
                key, json,
                TimeSpan.FromMinutes(10));
        }

        public async Task RemoveAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var key = $"product:{productId}";

            await _database.KeyDeleteAsync(key);
        }
    }
}
