using BuildingBlocks.Interfaces;
using ProductService.Domain.Entities.Products;

namespace ProductService.Application.Interfaces
{
    public interface IProductRepository : IGenericRepositoryAsync<Product>
    {
        public IQueryable<Product> GetQueryable();
    }
}
