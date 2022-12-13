namespace Common.Application;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class HttpResultResponse : IActionResult
{
    public object? Body { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string ContentType { get; set; } = MediaTypeNames.Text.Plain;

    public static readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public async Task ExecuteResultAsync(ActionContext context)
    {
        if (Body is not null)
        {
            string fromSerializer = JsonSerializer.Serialize(Body, options: serializerOptions);

            context.HttpContext.Response.ContentLength = fromSerializer.Length;

            if (Body is ProblemDetails)
            {
                context.HttpContext.Response.StatusCode = ((ProblemDetails)Body).Status ?? 418;
                context.HttpContext.Response.ContentType = "application/problem+json";
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)StatusCode;
                context.HttpContext.Response.ContentType = ContentType;
            }

            await context.HttpContext.Response.WriteAsync(fromSerializer, context.HttpContext.RequestAborted);
            return;
        }

        string defaultPayload = HttpStatusText.From(StatusCode);

        context.HttpContext.Response.StatusCode = (int)StatusCode;
        context.HttpContext.Response.ContentType = MediaTypeNames.Text.Plain;
        context.HttpContext.Response.ContentLength = defaultPayload.Length;

        await context.HttpContext.Response.WriteAsync(defaultPayload, context.HttpContext.RequestAborted);
        return;
    }

    public async Task ExecuteResultOnHttpContextAsync(HttpContext context)
    {
        if (Body is not null)
        {
            string fromSerializer = JsonSerializer.Serialize(Body, options: serializerOptions);

            context.Response.ContentLength = fromSerializer.Length;

            if (Body is ProblemDetails)
            {
                context.Response.StatusCode = ((ProblemDetails)Body).Status ?? 418;
                context.Response.ContentType = "application/problem+json";
            }
            else
            {
                context.Response.StatusCode = (int)StatusCode;
                context.Response.ContentType = ContentType;
            }

            await context.Response.WriteAsync(fromSerializer, context.RequestAborted);
            return;
        }

        string defaultPayload = HttpStatusText.From(StatusCode);

        context.Response.StatusCode = (int)StatusCode;
        context.Response.ContentType = MediaTypeNames.Text.Plain;
        context.Response.ContentLength = defaultPayload.Length;

        await context.Response.WriteAsync(defaultPayload, context.RequestAborted);
        return;
    }
}
