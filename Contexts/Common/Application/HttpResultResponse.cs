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
        if (Body is null)
        {
            string defaultPayload = HttpStatusText.From(StatusCode);

            context.HttpContext.Response.StatusCode = (int)StatusCode;
            context.HttpContext.Response.ContentType = MediaTypeNames.Text.Plain;
            context.HttpContext.Response.ContentLength = defaultPayload.Length;

            await context.HttpContext.Response.WriteAsync(defaultPayload, context.HttpContext.RequestAborted);
            return;
        }

        if (Body is ProblemDetails)
        {
            string s0 = JsonSerializer.Serialize(Body, options: serializerOptions);

            context.HttpContext.Response.StatusCode = ((ProblemDetails)Body).Status ?? 418;
            context.HttpContext.Response.ContentType = "application/problem+json";
            context.HttpContext.Response.ContentLength = s0.Length;

            await context.HttpContext.Response.WriteAsync(s0, context.HttpContext.RequestAborted);
            return;
        }

        string s1 = JsonSerializer.Serialize(Body, options: serializerOptions);

        context.HttpContext.Response.StatusCode = (int)StatusCode;
        context.HttpContext.Response.ContentType = ContentType;
        context.HttpContext.Response.ContentLength = s1.Length;

        await context.HttpContext.Response.WriteAsync(s1, context.HttpContext.RequestAborted);
    }
}
