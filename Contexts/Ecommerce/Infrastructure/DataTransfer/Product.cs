namespace Ecommerce.Infrastructure.DataTransfer;

public readonly struct CreateProductHttpRequestBody
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Price { get; init; }
    public required string Status { get; init; }
}

public readonly struct UpdateProductHttpRequestBody
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Price { get; init; }
    public string? Status { get; init; }
}