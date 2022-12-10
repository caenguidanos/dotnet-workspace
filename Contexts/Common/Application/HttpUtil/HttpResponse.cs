namespace Common.Application.HttpUtil;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

public class HttpResultResponse : ActionResult
{
    public required int StatusCode { get; set; }
    public string ContentType { get; set; } = MediaTypeNames.Text.Plain;
    public object? Body { get; set; }

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
        string payload;

        if (Body is null)
        {
            payload = JsonSerializer.Serialize(
                new { Message = HttpStatusText.From(StatusCode) }, options: _jsonSerializerOptions);

            context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
        }
        else
        {
            payload = JsonSerializer.Serialize(Body, options: _jsonSerializerOptions);

            context.HttpContext.Response.ContentType = ContentType;
        }

        context.HttpContext.Response.StatusCode = StatusCode;

        await context.HttpContext.Response.WriteAsync(payload, _cancellationToken);
    }
}