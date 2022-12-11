namespace Common.Application.HttpUtil;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class HttpResultResponse : ActionResult
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

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)StatusCode;

        if (Body is null)
        {
            context.HttpContext.Response.ContentType = MediaTypeNames.Text.Plain;
            await context.HttpContext.Response.WriteAsync(HttpStatusText.From(StatusCode), _cancellationToken);
            return;
        }

        if (Body is string or int or bool)
        {
            context.HttpContext.Response.ContentType = ContentType;
            await context.HttpContext.Response.WriteAsync(Body.ToString(), _cancellationToken);
            return;
        }

        string payload = JsonSerializer.Serialize(Body, options: _jsonSerializerOptions);
        context.HttpContext.Response.ContentType = ContentType;
        await context.HttpContext.Response.WriteAsync(payload, _cancellationToken);
        return;
    }
}
