namespace Common.Application.Exceptions;

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Common.Application.HttpUtil;

public class FallbackExceptionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is null || context.ExceptionHandled)
        {
            return;
        }

        context.Result = new HttpResultResponse()
        {
            ProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound
            }
        };

        context.ExceptionHandled = true;
    }
}
