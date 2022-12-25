namespace Ecommerce.Domain;

public sealed record Product : IAggregateRoot<Product, ProductPrimitives>
{
    public required ProductId Id { private get; init; }
    public required ProductTitle Title { private get; init; }
    public required ProductDescription Description { private get; init; }
    public required ProductStatus Status { private get; init; }
    public required ProductPrice Price { private get; init; }

    public ProductPrimitives ToPrimitives() =>
        new()
        {
            Id = Id.GetValue(),
            Title = Title.GetValue(),
            Description = Description.GetValue(),
            Status = Status.GetValue(),
            Price = Price.GetValue(),
        };

    public static Product FromPrimitives(ProductPrimitives productPrimitives) =>
        new()
        {
            Id = new ProductId(productPrimitives.Id),
            Title = new ProductTitle(productPrimitives.Title),
            Description = new ProductDescription(productPrimitives.Description),
            Status = new ProductStatus(productPrimitives.Status),
            Price = new ProductPrice(productPrimitives.Price),
        };
}

public readonly struct ProductPrimitives
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Price { get; init; }
    public required int Status { get; init; }
}