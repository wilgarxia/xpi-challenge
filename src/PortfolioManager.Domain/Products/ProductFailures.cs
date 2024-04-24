using ErrorOr;

namespace PortfolioManager.Domain.Products;

public static class ProductFailures
{
    public static readonly Error ProductAlreadyExists = Error.Validation("pro-0001", "Product already exists.");
    public static readonly Error ProductUnavailable = Error.Validation("pro-0002", "Product is unavailable.");
    public static readonly Error AmountLessThanMinimumRequired = Error.Validation("pro-0003", "The amount is less than the minimum required.");
}