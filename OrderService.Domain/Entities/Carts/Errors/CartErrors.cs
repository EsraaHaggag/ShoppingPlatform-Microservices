using BuildingBlocks.Common;

namespace OrderService.Domain.Entities.Carts.Errors
{
    public class CartErrors
    {
        public static readonly Error InvalidCustomer =
        Error.Validation(
            "Cart.InvalidCustomer",
            "Customer is invalid.");

        public static readonly Error InvalidProduct =
            Error.Validation(
                "Cart.InvalidProduct",
                "Product is invalid.");

        public static readonly Error InvalidProductName =
            Error.Validation(
                "Cart.InvalidProductName",
                "Product name is required.");

        public static readonly Error InvalidPrice =
            Error.Validation(
                "Cart.InvalidPrice",
                "Product price must be greater than zero.");

        public static readonly Error InvalidQuantity =
            Error.Validation(
                "Cart.InvalidQuantity",
                "Quantity must be greater than zero.");

        public static readonly Error ItemNotFound =
            Error.NotFound(
                "Cart.ItemNotFound",
                "Cart item was not found.");

        public static readonly Error EmptyCart =
            Error.Validation(
                "Cart.Empty",
                "Cart cannot be checked out because it is empty.");

        public static readonly Error InsufficientStock =
            Error.Conflict(
                "Cart.InsufficientStock",
                "The requested quantity is not available.");

        public static readonly Error ProductNotFound =
        Error.NotFound(
            "Product.NotFound",
            "Product was not found.");

        public static readonly Error ServiceUnavailable =
            new Error(
                "ProductService.Unavailable",
                "Product service is currently unavailable.",
                ErrorType.Unexpected);

        public static readonly Error InvalidResponse =
            new Error(
                "ProductService.InvalidResponse",
                "Invalid response from ProductService.",
                ErrorType.Unexpected);
    }
}
