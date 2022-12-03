namespace api.Contexts.Ecommerce.Store.Domain.Model
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() { }

        public ProductNotFoundException(string message) : base(message) { }

        public ProductNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}