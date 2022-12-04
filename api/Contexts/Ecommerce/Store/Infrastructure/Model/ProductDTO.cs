namespace api.Contexts.Ecommerce.Store.Infrastructure.Model
{
    public class CreateProductRequestBodyDTO
    {
        public required int Price { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Status { get; set; }
    }
}