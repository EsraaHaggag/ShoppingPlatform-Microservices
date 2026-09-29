using BuildingBlocks.Stock;
using OrderService.Application.Contracts.Payments;

namespace OrderService.Application.Features.Orders.Commands.Checkout
{
    public record CheckoutSagaResponse(
    bool Success,
    InitiatePaymentResponse? Payment,
    IReadOnlyList<StockReservationItemStatus>? ItemsStatus,
    string? Message);
}
