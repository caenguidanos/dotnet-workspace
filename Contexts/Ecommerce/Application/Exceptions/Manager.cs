namespace Ecommerce.Application.Exceptions;

using System.Net;
using Microsoft.AspNetCore.Mvc;

using Common.Application;

using Ecommerce.Domain.Exceptions;

public sealed class ExceptionManager : HttpExceptionManager
{
    public ExceptionManager()
    {
        exceptions.Add(nameof(ProductNotFoundException), new HttpResultResponse()
        {
            Body = new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound
            }
        });

        exceptions.Add(nameof(ProductTitleInvalidException), new HttpResultResponse()
        {
            Body = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        exceptions.Add(nameof(ProductTitleUniqueException), new HttpResultResponse()
        {
            Body = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        exceptions.Add(nameof(ProductDescriptionInvalidException), new HttpResultResponse()
        {
            Body = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        exceptions.Add(nameof(ProductPriceInvalidException), new HttpResultResponse()
        {
            Body = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        exceptions.Add(nameof(ProductStatusInvalidException), new HttpResultResponse()
        {
            Body = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });

        exceptions.Add(nameof(ProductPersistenceException), new HttpResultResponse()
        {
            Body = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest
            }
        });
    }
}