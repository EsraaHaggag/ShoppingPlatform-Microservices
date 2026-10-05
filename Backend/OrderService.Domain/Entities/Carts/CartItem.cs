using BuildingBlocks.Common;
using OrderService.Domain.Entities.Carts.Errors;

namespace OrderService.Domain.Entities.Carts
{
    public class CartItem : CommonData
    {
        public Guid Id { get; private set; }

        public Guid ProductId { get; private set; }

        public string ProductName { get; private set; } = string.Empty;

        public string? ProductImageUrl { get; private set; }

        public decimal UnitPrice { get; private set; }

        public int Quantity { get; private set; }

        public decimal TotalPrice =>
            UnitPrice * Quantity;

        private CartItem() { }
        private CartItem(Guid productId, string productName, decimal unitPrice,
          int quantity, string? productImageUrl)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            ProductImageUrl = productImageUrl;
        }

        public static Result<CartItem> Create(Guid productId, string productName, decimal unitPrice, int quantity, string? productImageUrl = null)
        {
            var item = new CartItem(productId, productName, unitPrice, quantity, productImageUrl);

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
