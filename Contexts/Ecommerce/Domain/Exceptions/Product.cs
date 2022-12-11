namespace Ecommerce.Domain.Exceptions;

public sealed class ProductNotFoundException : Exception
{
    public ProductNotFoundException()
    {
    }

    public ProductNotFoundException(string paramName)
        : base(paramName)
    {
    }

    public ProductNotFoundException(string paramName, Exception inner)
        : base(paramName, inner)
    {
    }
}

public sealed class ProductTitleInvalidException : Exception
{
    public ProductTitleInvalidException()
    {
    }

    public ProductTitleInvalidException(string paramName)
        : base(paramName)
    {
    }

    public ProductTitleInvalidException(string paramName, Exception inner)
        : base(paramName, inner)
    {
    }
}

public sealed class ProductDescriptionInvalidException : Exception
{
    public ProductDescriptionInvalidException()
    {
    }

    public ProductDescriptionInvalidException(string paramName)
        : base(paramName)
    {
    }

    public ProductDescriptionInvalidException(string paramName, Exception inner)
        : base(paramName, inner)
    {
    }
}

public sealed class ProductPriceInvalidException : Exception
{
    public ProductPriceInvalidException()
    {
    }

    public ProductPriceInvalidException(string paramName)
        : base(paramName)
    {
    }

    public ProductPriceInvalidException(string paramName, Exception inner)
        : base(paramName, inner)
    {
    }
}

public sealed class ProductStatusInvalidException : Exception
{
    public ProductStatusInvalidException()
    {
    }

    public ProductStatusInvalidException(string paramName)
        : base(paramName)
    {
    }

    public ProductStatusInvalidException(string paramName, Exception inner)
        : base(paramName, inner)
    {
    }
}

public sealed class ProductPersistenceException : Exception
{
    public ProductPersistenceException()
    {
    }

    public ProductPersistenceException(string paramName)
        : base(paramName)
    {
    }

    public ProductPersistenceException(string paramName, Exception inner)
        : base(paramName, inner)
    {
    }
}
