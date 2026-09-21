using BuildingBlocks.Common;
using ProductService.Domain.Entities.Products.Errors;


namespace ProductService.Domain.Entities.Products;

public class Product : CommonData
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }

    private Product(
        string name,
        string description,
        decimal price,
        int stockQuantity)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public static Result<Product> Create(
    string name,
    string description,
    decimal price,
    int stockQuantity)
    {
        if (price <= 0)
            return Result<Product>.Failure(ProductErrors.InvalidPrice);

        if (stockQuantity < 0)
            return Result<Product>.Failure(ProductErrors.InvalidQuantity);

        var product = new Product(
            name,
            description,
            price,
            stockQuantity);

        return Result<Product>.Success(product);
    }
    public Result UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            return Result.Failure(ProductErrors.InvalidPrice);

        Price = newPrice;

        return Result.Success();
    }

    public Result DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(ProductErrors.InvalidQuantity);

        if (quantity > StockQuantity)
            return Result.Failure(ProductErrors.InsufficientStock);

        StockQuantity -= quantity;

        return Result.Success();
    }

    public Result IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(ProductErrors.InvalidQuantity);

        StockQuantity += quantity;

        return Result.Success();
    }
    public Result Deactivate(string? deletedBy = null)
    {
        if (IsDeleted)
            return Result.Failure(ProductErrors.AlreadyDeactivated);

        SoftDelete(deletedBy);
        return Result.Success();
    }
    public Result Reactivate()
    {
        if (!IsDeleted)
            return Result.Failure(ProductErrors.AlreadyActive);

        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;

        return Result.Success();
    }
}