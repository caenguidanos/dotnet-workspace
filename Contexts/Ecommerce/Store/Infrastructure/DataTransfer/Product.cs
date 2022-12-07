namespace Ecommerce.Store.Infrastructure.DataTransfer;

using Common.Domain.Entity;

public class ProductAck
{
    public required Guid Id { get; set; }
}

public class ProductPrimitives : SchemaPrimitives
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required int Status { get; set; }
}


public class ProductPartialPrimitivesWithoutSchema
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required int Status { get; set; }
}

public class ProductPartialOptionalPrimitivesWithoutSchema
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Price { get; set; }
    public int? Status { get; set; }
}

