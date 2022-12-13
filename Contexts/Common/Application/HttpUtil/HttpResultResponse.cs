namespace Common.Application.HttpUtil;

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

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)StatusCode;

        if (Body is null)
        {
            string defaultPayload = HttpStatusText.From(StatusCode);
            context.HttpContext.Response.ContentLength = defaultPayload.Length;
            context.HttpContext.Response.ContentType = MediaTypeNames.Text.Plain;
            await context.HttpContext.Response.WriteAsync(defaultPayload, context.HttpContext.RequestAborted);
            return;
        }

        if (Body is string or int or bool)
        {
            string stringifiedPayload = Body.ToString() ?? string.Empty;
            context.HttpContext.Response.ContentLength = stringifiedPayload.Length;
            context.HttpContext.Response.ContentType = ContentType;
            await context.HttpContext.Response.WriteAsync(stringifiedPayload, context.HttpContext.RequestAborted);
            return;
        }

        string fromSerializerPayload = JsonSerializer.Serialize(Body, options: _jsonSerializerOptions);
        context.HttpContext.Response.ContentLength = fromSerializerPayload.Length;
        context.HttpContext.Response.ContentType = ContentType ?? MediaTypeNames.Application.Json;
        await context.HttpContext.Response.WriteAsync(fromSerializerPayload, context.HttpContext.RequestAborted);
        return;
    }
}
