namespace Ecommerce.IntegrationTest;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public static class IntegrationTestHelpers
{
    public static ActionContext CreateWritableActionContext()
    {
        var actionContext = new ActionContext();
        actionContext.HttpContext = new DefaultHttpContext();
        actionContext.HttpContext.Response.Body = new MemoryStream();

        return actionContext;
    }

    public static string ReadBodyFromActionContext(ActionContext actionContext, bool resetPointer)
    {
        if (resetPointer)
        {
            actionContext.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        }

        var responseBodyReader = new StreamReader(actionContext.HttpContext.Response.Body);
        return responseBodyReader.ReadToEnd();
    }
}