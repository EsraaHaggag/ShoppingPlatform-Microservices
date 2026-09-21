using BuildingBlocks.Common;
using OrderService.Domain.Entities.Carts.Errors;

namespace OrderService.Domain.Entities.Carts
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items =>
            _items.AsReadOnly();
        public decimal TotalAmount =>
            _items.Sum(x => x.TotalPrice);
        private Cart() { }

        private Cart(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
        }

        public static Result<Cart> Create(Guid customerId)
        {
            if (customerId == Guid.Empty)
                return Result<Cart>.Failure(
                    CartErrors.InvalidCustomer);

            var cart = new Cart(customerId);

            return Result<Cart>.Success(cart);
        }

        public Result AddItem(Guid productId,
        string productName, decimal unitPrice,
        int quantity)
        {
            if (quantity <= 0)
                return Result.Failure(
                    CartErrors.InvalidQuantity);

            var existingItem = _items
                .FirstOrDefault(x => x.ProductId == productId);

            if (existingItem is not null)
            {
                var newQuantity = existingItem.Quantity + quantity;

                return existingItem.UpdateQuantity(newQuantity);
            }

            var item = CartItem.Create(productId, productName, unitPrice,
                quantity);
            if (item.IsFailure)
                return Result.Failure(item.Error);
            _items.Add(item.Value);

            return Result.Success();
        }

        public Result RemoveItem(Guid itemId)
        {
            var item = _items
                .FirstOrDefault(x => x.Id == itemId);
            if (item is null)
                return Result.Failure(
                    CartErrors.ItemNotFound);
            _items.Remove(item);
            return Result.Success();
        }

        public Result UpdateQuantity(Guid itemId, int quantity)
        {
            if (quantity <= 0)
                return Result.Failure(
                    CartErrors.InvalidQuantity);
            var item = _items
                .FirstOrDefault(x => x.Id == itemId);
            if (item is null)
                return Result.Failure(
                    CartErrors.ItemNotFound);
            return item.UpdateQuantity(quantity);
        }
        public void Clear()
        {
            _items.Clear();
        }
    }
}