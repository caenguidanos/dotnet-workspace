namespace Ecommerce.Store.Domain.Model;

public class ProductNotFoundException : Exception
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

public class ProductIdInvalidException : Exception
{
    public ProductIdInvalidException()
    {
    }

    public ProductIdInvalidException(string paramName)
        : base(paramName)
    {
    }

    public ProductIdInvalidException(string paramName, Exception inner)
        : base(paramName, inner)
    {
    }
}

public class ProductTitleInvalidException : Exception
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

public class ProductDescriptionInvalidException : Exception
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

public class ProductPriceInvalidException : Exception
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

public class ProductStatusInvalidException : Exception
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