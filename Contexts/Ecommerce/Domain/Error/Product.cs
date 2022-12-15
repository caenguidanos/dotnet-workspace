namespace Ecommerce.Domain.Error;

using System.Net;

using Common.Domain;

public class ProductError : Exception
{ }

public sealed class ProductNotFoundError : ProductError, IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.NotFound;
    public string Detail { get; } = "NotFound";
}

public sealed class ProductTitleInvalidError : ProductError, IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}

public sealed class ProductTitleUniqueError : ProductError, IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}

public sealed class ProductDescriptionInvalidError : ProductError, IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}

public sealed class ProductPriceInvalidError : ProductError, IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}

public sealed class ProductStatusInvalidError : ProductError, IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}


public sealed class ProductPersistenceError : ProductError, IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.ServiceUnavailable;
    public string Detail { get; } = "ServiceUnavailable";
}
