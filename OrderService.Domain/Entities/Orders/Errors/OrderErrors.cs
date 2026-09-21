using BuildingBlocks.Common;

namespace OrderService.Domain.Entities.Orders.Errors
{
    public static class OrderErrors
    {
        public static readonly Error InvalidCustomer =
        Error.Validation(
            "Order.InvalidCustomer",
            "Customer is invalid.");

        public static readonly Error InvalidProduct =
            Error.Validation(
                "Order.InvalidProduct",
                "Product is invalid.");

        public static readonly Error InvalidPrice =
            Error.Validation(
                "Order.InvalidPrice",
                "Price must be greater than zero.");

        public static readonly Error InvalidQuantity =
            Error.Validation(
                "Order.InvalidQuantity",
                "Quantity must be greater than zero.");

        public static readonly Error AlreadyCancelled =
            Error.Conflict(
                "Order.AlreadyCancelled",
                "Order is already cancelled.");

        public static readonly Error CannotCancel =
            Error.Conflict(
                "Order.CannotCancel",
                "Completed order cannot be cancelled.");
    }
}
