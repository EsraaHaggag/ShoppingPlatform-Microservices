namespace OrderService.Domain.Enums
{
    public enum OrderSagaStatus
    {
        Started,

        StockReservationPending,
        StockReserved,

        PaymentPending,
        PaymentSucceeded,

        Completed,

        Compensating,
        Compensated,

        Failed
    }
}
