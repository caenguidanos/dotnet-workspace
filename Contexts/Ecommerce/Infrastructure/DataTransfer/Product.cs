namespace Ecommerce.Infrastructure;

using Common.Domain;

using Domain;

public sealed record ProductPrimitives : SchemaPrimitives
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required ProductStatusValue Status { get; set; }
}

public struct CreateProductHttpRequestBody
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required int Status { get; set; }
}

public struct UpdateProductHttpRequestBody
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Price { get; set; }
    public int? Status { get; set; }
}