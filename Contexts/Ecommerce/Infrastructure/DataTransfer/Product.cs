namespace Ecommerce.Infrastructure;

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