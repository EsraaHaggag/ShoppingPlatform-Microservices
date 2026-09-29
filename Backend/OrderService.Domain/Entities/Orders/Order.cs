using BuildingBlocks.Common;
using OrderService.Domain.Entities.Orders.Errors;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities.Orders
{
    public class Order : CommonData
    {
        public Guid Id { get; private set; }

        public Guid CustomerId { get; private set; }

        public decimal TotalAmount { get; private set; }

        public OrderStatus Status { get; private set; }

        private readonly List<OrderItem> _items = new();

        public IReadOnlyCollection<OrderItem> Items =>
            _items.AsReadOnly();

        private Order() { }
        private Order(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<Order> Create(Guid customerId)
        {
            if (customerId == Guid.Empty)
                return Result<Order>.Failure(
                  OrderErrors.InvalidCustomer);

            var order = new Order(customerId);

            return Result<Order>.Success(order);
        }
        public Result AddItem(Guid productId,
        string productName, decimal unitPrice,
        int quantity)
        {
            if (productId == Guid.Empty)
                return Result.Failure(OrderErrors.InvalidProduct);

            if (unitPrice <= 0)
                return Result.Failure(OrderErrors.InvalidPrice);

            if (quantity <= 0)
                return Result.Failure(OrderErrors.InvalidQuantity);

            var item = OrderItem.Create(
                productId, productName,
                unitPrice, quantity);

            if (item.IsFailure)
                return Result.Failure(item.Error);

            _items.Add(item.Value);

            TotalAmount += item.Value.TotalPrice;

            return Result.Success();
        }

        public Result Cancel()
        {
            if (Status == OrderStatus.Cancelled)
                return Result.Failure(OrderErrors.AlreadyCancelled);
            if (Status == OrderStatus.Completed)
                return Result.Failure(OrderErrors.CannotCancel);
            Status = OrderStatus.Cancelled;
            return Result.Success();
        }


        private void RecalculateTotal()
        {
            TotalAmount = _items.Sum(x => x.TotalPrice);
        }

        public Result MarkAsPaid()
        {
            if (Status != OrderStatus.Pending)
            {
                return Result.Failure(
                    new Error(
                        "Order.InvalidStatus",
                        "Order cannot be marked as paid.",
                        ErrorType.Validation));
            }

            Status = OrderStatus.Completed;

            return Result.Success();
        }

    }
}
