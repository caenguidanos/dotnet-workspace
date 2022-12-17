namespace Ecommerce.Domain.Error;

using System.Net;
using Common.Domain;

public class ProductException : Exception, IHttpError
{
    public int StatusCode { get; set; }
    public string Detail { get; set; } = string.Empty;
}

public sealed class ProductNotFoundException : ProductException
{
    public new int StatusCode { get; } = (int)HttpStatusCode.NotFound;
    public new string Detail { get; } = "NotFound";
}

public sealed class ProductTitleInvalidException : ProductException
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}

public sealed class ProductTitleUniqueException : ProductException
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}

public sealed class ProductDescriptionInvalidException : ProductException
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}

public sealed class ProductPriceInvalidException : ProductException
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}

public sealed class ProductStatusInvalidException : ProductException
{
    public new int StatusCode { get; } = (int)HttpStatusCode.BadRequest;
    public new string Detail { get; } = "BadRequest";
}

public sealed class ProductPersistenceException : ProductException
{
    public new int StatusCode { get; } = (int)HttpStatusCode.ServiceUnavailable;
    public new string Detail { get; } = "ServiceUnavailable";
}
