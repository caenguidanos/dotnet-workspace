namespace Ecommerce.Infrastructure;

public struct ProductConstraints
{
    public const string UniqueTitle = "product_title_key";
    public const string CheckStatus = "check_status_value";
    public const string CheckPrice = "check_price_range";
    public const string CheckTitle = "check_title_length";
    public const string CheckDescription = "check_description_length";
}