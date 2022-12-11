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
    public required HttpStatusCode StatusCode { get; set; }
    public string ContentType { get; set; } = MediaTypeNames.Text.Plain;

    private CancellationToken _cancellationToken { get; init; }

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public HttpResultResponse(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)StatusCode;

        if (Body is null)
        {
            string defaultPayload = HttpStatusText.From(StatusCode);
            context.HttpContext.Response.ContentLength = defaultPayload.Length;
            context.HttpContext.Response.ContentType = MediaTypeNames.Text.Plain;
            await context.HttpContext.Response.WriteAsync(defaultPayload, _cancellationToken);
            return;
        }

        if (Body is string or int or bool)
        {
            string stringifiedPayload = Body.ToString() ?? string.Empty;
            context.HttpContext.Response.ContentLength = stringifiedPayload.Length;
            context.HttpContext.Response.ContentType = ContentType;
            await context.HttpContext.Response.WriteAsync(stringifiedPayload, _cancellationToken);
            return;
        }

        string fromSerializerPayload = JsonSerializer.Serialize(Body, options: _jsonSerializerOptions);
        context.HttpContext.Response.ContentLength = fromSerializerPayload.Length;
        context.HttpContext.Response.ContentType = ContentType;
        await context.HttpContext.Response.WriteAsync(fromSerializerPayload, _cancellationToken);
        return;
    }
}
