namespace Common.Application;

using System.Net;
using Microsoft.AspNetCore.Mvc;

public interface IHttpExceptionManager
{
    public HttpResultResponse Intercept(Exception ex);
}

public class HttpExceptionManager : IHttpExceptionManager
{
    protected Dictionary<string, HttpResultResponse> exceptions = new();

    public HttpResultResponse Intercept(Exception ex)
    {
        HttpResultResponse? result;

        exceptions.TryGetValue(ex.GetType().Name, out result);

        if (result is null)
        {
            result = new HttpResultResponse()
            {
                Body = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotImplemented
                }
            };
        }

        return result;
    }
}