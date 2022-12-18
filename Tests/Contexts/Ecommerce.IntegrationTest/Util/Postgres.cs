namespace Ecommerce.IntegrationTest.Util;

using Dapper;
using Npgsql;

public static class PostgresUtil
{
    public static async Task ExecuteAsync(string connectionString, string sql)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        await conn.ExecuteAsync(sql);
        await conn.CloseAsync();
    }
}