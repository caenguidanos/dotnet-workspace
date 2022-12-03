namespace api.Contexts.Ecommerce.Store.Domain.Model
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() { }

        public ProductNotFoundException(string message) : base(message) { }

        public ProductNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class ProductIdInvalidException : Exception
    {
        public ProductIdInvalidException() { }

        public ProductIdInvalidException(string message) : base(message) { }

        public ProductIdInvalidException(string message, Exception inner) : base(message, inner) { }
    }

    public class ProductTitleInvalidException : Exception
    {
        public ProductTitleInvalidException() { }

        public ProductTitleInvalidException(string message) : base(message) { }

        public ProductTitleInvalidException(string message, Exception inner) : base(message, inner) { }
    }

    public class ProductDescriptionInvalidException : Exception
    {
        public ProductDescriptionInvalidException() { }

        public ProductDescriptionInvalidException(string message) : base(message) { }

        public ProductDescriptionInvalidException(string message, Exception inner) : base(message, inner) { }
    }

    public class ProductPriceInvalidException : Exception
    {
        public ProductPriceInvalidException() { }

        public ProductPriceInvalidException(string message) : base(message) { }

        public ProductPriceInvalidException(string message, Exception inner) : base(message, inner) { }
    }

    public class ProductStatusInvalidException : Exception
    {
        public ProductStatusInvalidException() { }

        public ProductStatusInvalidException(string message) : base(message) { }

        public ProductStatusInvalidException(string message, Exception inner) : base(message, inner) { }
    }
}