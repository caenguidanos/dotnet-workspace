namespace Ecommerce.Store.Infrastructure.DataTransfer;

using Common.Domain;

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

public class ProductEventPrimitives : SchemaPrimitives
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required Guid Owner { get; set; }
}

public class ProductPrimitivesForCreateOperation
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required int Status { get; set; }
}

public class ProductPrimitivesForUpdateOperation
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Price { get; set; }
    public int? Status { get; set; }
}

