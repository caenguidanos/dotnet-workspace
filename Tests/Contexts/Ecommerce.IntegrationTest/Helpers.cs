namespace Ecommerce.IntegrationTest;

using System.Text.Json;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

public static class IntegrationTestHelpers
{
    public static async Task ExecuteQueryAsync(string connectionString, string sql)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        await conn.ExecuteAsync(sql);
        await conn.CloseAsync();
    }

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

    public static T? JsonDeserialize<T>(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(json, options);
    }
}