namespace Ecommerce.IntegrationTest.Fixture;

using Dapper;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

using Npgsql;

using Moq;

using Common.Fixture.Infrastructure.Database;

using Microsoft.AspNetCore.TestHost;

using Infrastructure;

public class WebAppFactory : WebApplicationFactory<Program>
{
    private string _connectionString { get; set; } = string.Empty;

    private readonly PostgresDatabaseFactory _postgres = new("ecommerce");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connectionString = _postgres.StartAsync().Result;

        builder
            .UseEnvironment("Release")
            .ConfigureTestServices(servicesConfiguration =>
            {
                var dbContext = servicesConfiguration
                    .SingleOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(IDbContext));

                if (dbContext is not null) servicesConfiguration.Remove(dbContext);

                var customDbContext = Mock.Of<IDbContext>();

                Mock.Get(customDbContext)
                    .Setup(c => c.GetConnectionString())
                    .Returns(_connectionString);

                servicesConfiguration.AddSingleton(customDbContext);
            });
    }

    public async Task ExecuteSqlAsync(string sql)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await conn.ExecuteAsync(sql);
        await conn.CloseAsync();
    }
}