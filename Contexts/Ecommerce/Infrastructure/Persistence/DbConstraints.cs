namespace Ecommerce.Infrastructure.Persistence;

public struct ProductConstraints
{
    public const string UniqueTitle = "product_title_key";
    public const string CheckStatus = "product_status_check";
    public const string CheckPrice = "product_price_check";
    public const string CheckTitle = "product_title_check";
    public const string CheckDescription = "product_description_check";
}