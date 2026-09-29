using BuildingBlocks.Common;

namespace ProductService.Domain.Entities.Products.Errors
{
    public static class ProductErrors
    {
        public static readonly Error InvalidPrice =
            Error.Validation(
                "Product.InvalidPrice",
                "Price must be greater than zero.");

        public static readonly Error InvalidQuantity =
            Error.Validation(
                "Product.InvalidQuantity",
                "Quantity must be greater than zero.");

        public static readonly Error InsufficientStock =
            Error.Conflict(
                "Product.InsufficientStock",
                "Not enough stock.");

        public static readonly Error ProductNotFound =
           Error.NotFound(
              "Product.NotFound",
              "Product was not found.");

        public static readonly Error AlreadyDeactivated =
            Error.Conflict(
                "Product.AlreadyDeactivated",
                "Product is already deactivated.");

        public static readonly Error AlreadyActive =
            Error.Conflict(
                "Product.AlreadyActive",
                "Product is already active.");
    }
}
