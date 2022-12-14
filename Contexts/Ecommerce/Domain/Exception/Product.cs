namespace Ecommerce.Domain.Exception;

public sealed class ProductNotFoundException : ProblemDetailsException
{
    public ProductNotFoundException()
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.NotFound));
        SetDetail("Product not found");
        SetStatusCode(HttpStatusCode.NotFound);
    }
}

public sealed class ProductIdUniqueException : ProblemDetailsException
{
    public ProductIdUniqueException()
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.BadRequest));
        SetDetail("Product id is not unique");
        SetStatusCode(HttpStatusCode.BadRequest);
    }
}

public sealed class ProductTitleUniqueException : ProblemDetailsException
{
    public ProductTitleUniqueException()
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.BadRequest));
        SetDetail("Product title is not unique");
        SetStatusCode(HttpStatusCode.BadRequest);
    }
}

public sealed class ProductIdInvalidException : ProblemDetailsException
{
    public ProductIdInvalidException()
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.BadRequest));
        SetDetail("Product id is invalid");
        SetStatusCode(HttpStatusCode.BadRequest);
    }
}

public sealed class ProductTitleInvalidException : ProblemDetailsException
{
    public ProductTitleInvalidException()
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.BadRequest));
        SetDetail("Product title is invalid");
        SetStatusCode(HttpStatusCode.BadRequest);
    }
}

public sealed class ProductDescriptionInvalidException : ProblemDetailsException
{
    public ProductDescriptionInvalidException()
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.BadRequest));
        SetDetail("Product description is invalid");
        SetStatusCode(HttpStatusCode.BadRequest);
    }
}

public sealed class ProductPriceInvalidException : ProblemDetailsException
{
    public ProductPriceInvalidException()
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.BadRequest));
        SetDetail("Product price is out of range");
        SetStatusCode(HttpStatusCode.BadRequest);
    }
}

public sealed class ProductStatusInvalidException : ProblemDetailsException
{
    public ProductStatusInvalidException()
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.BadRequest));
        SetDetail("Product status is invalid");
        SetStatusCode(HttpStatusCode.BadRequest);
    }
}

public sealed class ProductPersistenceException : ProblemDetailsException
{
    public ProductPersistenceException(string detail)
    {
        SetTitle(HttpStatusText.From(HttpStatusCode.ServiceUnavailable));
        SetDetail(detail);
        SetStatusCode(HttpStatusCode.ServiceUnavailable);
    }
}