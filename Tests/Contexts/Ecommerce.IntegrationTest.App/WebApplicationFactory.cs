namespace Ecommerce.IntegrationTest.App;

using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;

using Common.Fixture.Infrastructure.Database;

public class WebAppFactory : WebApplicationFactory<Program>
{
    private readonly PostgresDatabaseFactory _postgres = new(template: "ecommerce");

    private string _connectionString { get; set; } = string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Release");
    }

    public async Task StartDatabaseAsync()
    {
        _connectionString = await _postgres.StartAsync().ConfigureAwait(false);

        Environment.SetEnvironmentVariable("ConnectionStrings:Ecommerce", _connectionString);
    }

    public async Task ExecuteSqlAsync(string sql)
    {
        await using var conn = new NpgsqlConnection(_connectionString);

        await conn.OpenAsync();
        await conn.ExecuteAsync(sql);
        await conn.CloseAsync();
    }
}