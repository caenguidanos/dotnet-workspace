namespace api.Contexts.Ecommerce.Store.Infrastructure.Model
{
    class ProductPrimitives
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Price { get; set; }
        public required int Status { get; set; }
    }
}