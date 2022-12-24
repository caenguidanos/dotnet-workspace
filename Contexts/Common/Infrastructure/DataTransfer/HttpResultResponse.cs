namespace Common.Infrastructure;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class HttpResultResponse : IActionResult
{
    public object? Body { get; init; }
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
    public string ContentType { get; init; } = MediaTypeNames.Text.Plain;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task ExecuteResultAsync(ActionContext context)
    {
        switch (Body)
        {
            case null:
            {
                var defaultPayload = HttpStatusText.From(StatusCode);

                context.HttpContext.Response.StatusCode = (int)StatusCode;
                context.HttpContext.Response.ContentType = MediaTypeNames.Text.Plain;
                context.HttpContext.Response.ContentLength = defaultPayload.Length;

                await context.HttpContext.Response.WriteAsync(defaultPayload, context.HttpContext.RequestAborted);
                return;
            }

            case ProblemDetails details:
            {
                var s0 = JsonSerializer.Serialize(Body, SerializerOptions);

                context.HttpContext.Response.StatusCode = details.Status ?? 418;
                context.HttpContext.Response.ContentType = "application/problem+json";
                context.HttpContext.Response.ContentLength = s0.Length;

                await context.HttpContext.Response.WriteAsync(s0, context.HttpContext.RequestAborted);
                return;
            }
        }

        var s1 = JsonSerializer.Serialize(Body, SerializerOptions);

        context.HttpContext.Response.StatusCode = (int)StatusCode;
        context.HttpContext.Response.ContentType = ContentType;
        context.HttpContext.Response.ContentLength = s1.Length;

        await context.HttpContext.Response.WriteAsync(s1, context.HttpContext.RequestAborted);
    }
}