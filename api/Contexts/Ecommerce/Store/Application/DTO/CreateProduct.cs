namespace api.Contexts.Ecommerce.Store.Application.DTO
{
    public class CreateProductDTO
    {
        public required string Id { get; set; }
        public required int Price { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required int Status { get; set; }
    }
}