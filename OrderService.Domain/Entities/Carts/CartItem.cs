using BuildingBlocks.Common;
using OrderService.Domain.Entities.Carts.Errors;

namespace OrderService.Domain.Entities.Carts
{
    public class CartItem
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice => UnitPrice * Quantity;
        private CartItem() { }
        private CartItem(Guid productId, string productName, decimal unitPrice,
          int quantity)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public static Result<CartItem> Create(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            var item = new CartItem(productId, productName, unitPrice, quantity);

            return Result<CartItem>.Success(item);
        }

        public Result UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                return Result.Failure(
                    CartErrors.InvalidQuantity);
            Quantity = quantity;
            return Result.Success();
        }
    }
}
