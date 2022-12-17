namespace Ecommerce.Domain.Exception;

using System.Net;

using Common.Domain;

public sealed class ProductNotFoundException : ProblemDetailsException
{
    public ProductNotFoundException()
    {
        SetStatusCode(HttpStatusCode.NotFound);
        SetTitle("NotFound");
    }
}

public sealed class ProductIdInvalidException : ProblemDetailsException
{
    public ProductIdInvalidException()
    {
        SetStatusCode(HttpStatusCode.BadRequest);
        SetTitle("BadRequest");
    }
}

public sealed class ProductTitleInvalidException : ProblemDetailsException
{
    public ProductTitleInvalidException()
    {
        SetStatusCode(HttpStatusCode.BadRequest);
        SetTitle("BadRequest");
    }
}

public sealed class ProductTitleUniqueException : ProblemDetailsException
{
    public ProductTitleUniqueException()
    {
        SetStatusCode(HttpStatusCode.BadRequest);
        SetTitle("BadRequest");
    }
}

public sealed class ProductDescriptionInvalidException : ProblemDetailsException
{
    public ProductDescriptionInvalidException()
    {
        SetStatusCode(HttpStatusCode.BadRequest);
        SetTitle("BadRequest");
    }
}

public sealed class ProductPriceInvalidException : ProblemDetailsException
{
    public ProductPriceInvalidException()
    {
        SetStatusCode(HttpStatusCode.BadRequest);
        SetTitle("BadRequest");
    }
}

public sealed class ProductStatusInvalidException : ProblemDetailsException
{
    public ProductStatusInvalidException()
    {
        SetStatusCode(HttpStatusCode.BadRequest);
        SetTitle("BadRequest");
    }
}

public sealed class ProductPersistenceException : ProblemDetailsException
{
    public ProductPersistenceException(string detail)
    {
        SetStatusCode(HttpStatusCode.ServiceUnavailable);
        SetTitle("ServiceUnavailable");
        SetDetail(detail);
    }
}
