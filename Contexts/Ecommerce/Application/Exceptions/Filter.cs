namespace Ecommerce.Application.Exceptions;

using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Exceptions;

public class EcommerceExceptionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        switch (context.Exception)
        {
            case ProductNotFoundException:
                context.Result = new HttpResultResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                };

                context.ExceptionHandled = true;
                break;

            case ProductTitleInvalidException:
                context.Result = new HttpResultResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                };

                context.ExceptionHandled = true;
                break;

            case ProductTitleUniqueException:
                context.Result = new HttpResultResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                };

                context.ExceptionHandled = true;
                break;

            case ProductDescriptionInvalidException:
                context.Result = new HttpResultResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                };

                context.ExceptionHandled = true;
                break;

            case ProductPriceInvalidException:
                context.Result = new HttpResultResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                };

                context.ExceptionHandled = true;
                break;

            case ProductStatusInvalidException:
                context.Result = new HttpResultResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                };

                context.ExceptionHandled = true;
                break;

            case ProductPersistenceException:
                context.Result = new HttpResultResponse()
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                };


                context.ExceptionHandled = true;
                break;

            default:
                break;
        }
    }
}
