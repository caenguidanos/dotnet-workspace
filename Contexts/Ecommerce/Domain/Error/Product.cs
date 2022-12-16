namespace Ecommerce.Domain.Error;

using System.Net;

using Common.Domain;

public class ProductError : Exception, IError
{
    public int StatusCode { get; set; }
    public string Detail { get; set; } = string.Empty;
}

public sealed class ProductNotFoundError : ProductError
{
    public new int StatusCode { get; } = (int)HttpStatusCode.NotFound;
    public new string Detail { get; } = "NotFound";
}

public sealed class ProductTitleInvalidError : ProductError
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}

public sealed class ProductTitleUniqueError : ProductError
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}

public sealed class ProductDescriptionInvalidError : ProductError
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}

public sealed class ProductPriceInvalidError : ProductError
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}

public sealed class ProductStatusInvalidError : ProductError
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}


public sealed class ProductPersistenceError : ProductError
{
    public new int StatusCode { get; } = (int)HttpStatusCode.ServiceUnavailable;
    public new string Detail { get; } = "ServiceUnavailable";
}
