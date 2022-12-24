namespace Ecommerce.Domain;

public sealed record Product : Schema<ProductPrimitives>
{
    public required ProductId Id { private get; init; }
    public required ProductTitle Title { private get; init; }
    public required ProductDescription Description { private get; init; }
    public required ProductStatus Status { private get; init; }
    public required ProductPrice Price { private get; init; }

    public override ProductPrimitives ToPrimitives() =>
        new()
        {
            Id = Id.GetValue(),
            Title = Title.GetValue(),
            Description = Description.GetValue(),
            Status = Status.GetValue(),
            Price = Price.GetValue(),
            CreatedAt = CreatedAd,
            UpdatedAt = UpdatedAt
        };
}

public sealed record ProductPrimitives : SchemaPrimitives
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required ProductStatusValue Status { get; set; }
}