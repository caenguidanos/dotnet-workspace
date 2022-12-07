namespace Ecommerce.Store.Infrastructure.DataTransfer;

public class NewProduct
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required int Status { get; set; }
}

public class PartialProduct
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Price { get; set; }
    public int? Status { get; set; }
}

public class ProductPrimitives
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required int Status { get; set; }
    public required DateTime created_at { get; set; }
    public required DateTime updated_at { get; set; }
}

public class ProductAck
{
    public required Guid Id { get; set; }
}
