namespace Ecommerce.Domain.Error;

using System.Net;

using Common.Domain;

public sealed class ProductNotFoundError : IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.NotFound;
    public string Detail { get; } = "NotFound";
}

public sealed class ProductTitleInvalidError : IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}

public sealed class ProductTitleUniqueError : IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}

public sealed class ProductDescriptionInvalidError : IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}

public sealed class ProductPriceInvalidError : IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}

public sealed class ProductStatusInvalidError : IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public string Detail { get; } = "BadRequest";
}


public sealed class ProductPersistenceError : IError
{
    public int StatusCode { get; } = (int)HttpStatusCode.ServiceUnavailable;
    public string Detail { get; } = "ServiceUnavailable";
}
