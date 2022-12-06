namespace Ecommerce.Store.Infrastructure.DTO;

public class NewProduct
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required int Status { get; set; }
}
