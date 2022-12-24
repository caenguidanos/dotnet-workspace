namespace Ecommerce_IntegrationTesting_Program;

public class EcommerceWebApplicationFactory : WebApplicationFactory<Program>
{
    private string _connectionString { get; set; } = string.Empty;

    private readonly PostgresFactory _postgres = new("ecommerce");

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
    }
}