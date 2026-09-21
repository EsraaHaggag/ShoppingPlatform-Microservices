using BuildingBlocks.Common;
using OrderService.Domain.Entities.Orders.Errors;

namespace OrderService.Domain.Entities.Orders
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice => UnitPrice * Quantity;

        private OrderItem() { }

        private OrderItem(Guid productId,
        string productName, decimal unitPrice,
        int quantity)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public static Result<OrderItem> Create(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            var order = new OrderItem(productId, productName, unitPrice, quantity);

            return Result<OrderItem>.Success(order);
        }
        public Result UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                return Result.Failure(
                   OrderErrors.InvalidQuantity);
            Quantity = quantity;
            return Result.Success();
        }
    }
}
