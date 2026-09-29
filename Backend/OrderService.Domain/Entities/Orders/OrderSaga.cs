using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities.Orders
{
    public class OrderSaga
    {
        public Guid Id { get; private set; }

        public Guid OrderId { get; private set; }

        public Guid CustomerId { get; private set; }

        public OrderSagaStatus Status { get; private set; }

        public string? FailureReason { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        public DateTime? CompletedAtUtc { get; private set; }

        private OrderSaga() { }

        public OrderSaga(
            Guid orderId,
            Guid customerId)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            CustomerId = customerId;
            Status = OrderSagaStatus.Started;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void SetStatus(OrderSagaStatus status)
        {
            Status = status;
        }

        public void Fail(string reason)
        {
            Status = OrderSagaStatus.Failed;
            FailureReason = reason;
        }

        public void Complete()
        {
            Status = OrderSagaStatus.Completed;
            CompletedAtUtc = DateTime.UtcNow;
        }
    }
}
