namespace Ecommerce.Application.Exceptions;

using System.Net;
using Microsoft.AspNetCore.Mvc;

using Common.Application;

using Ecommerce.Domain.Exceptions;

public sealed class ExceptionManager : IExceptionManager
{
    private static Dictionary<string, HttpResultResponse> _exceptions = new();

    public ExceptionManager()
    {

        _exceptions.Add(nameof(ProductNotFoundException), new HttpResultResponse()
        {
            ProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound
            }
        });

        _exceptions.Add(nameof(ProductTitleInvalidException), new HttpResultResponse()
        {
            ProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        _exceptions.Add(nameof(ProductTitleUniqueException), new HttpResultResponse()
        {
            ProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        _exceptions.Add(nameof(ProductDescriptionInvalidException), new HttpResultResponse()
        {
            ProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        _exceptions.Add(nameof(ProductPriceInvalidException), new HttpResultResponse()
        {
            ProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        _exceptions.Add(nameof(ProductStatusInvalidException), new HttpResultResponse()
        {
            ProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        _exceptions.Add(nameof(ProductPersistenceException), new HttpResultResponse()
        {
            ProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

    }

    public HttpResultResponse HandleHttp(Exception ex)
    {
        HttpResultResponse? result;

        _exceptions.TryGetValue(nameof(ex), out result);

        if (result is null)
        {
            result = new HttpResultResponse()
            {
                ProblemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotImplemented
                }
            };
        }

        return result;
    }
}