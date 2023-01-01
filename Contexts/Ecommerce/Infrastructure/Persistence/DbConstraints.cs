namespace Ecommerce.Infrastructure.Persistence;

public readonly ref struct ProductConstraints
{
    public const string UniqueId = "product_pkey";
    public const string UniqueTitle = "product_title_key";
}