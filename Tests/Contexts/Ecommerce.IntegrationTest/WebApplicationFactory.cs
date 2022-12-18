namespace Ecommerce.IntegrationTest;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

using Common.Fixture.Infrastructure.Database;

public class EcommerceWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly PostgresDatabaseFactory _postgres = new(template: "ecommerce");
    public string PostgresDatabaseConnectionString { get; private set; } = string.Empty;

    public static readonly List<string> PostgresDatabaseInitScripts = new()
    {
        $"{Environment.CurrentDirectory}/SQL/Definitions:/var/lib/sql/denifitions",
        $"{Environment.CurrentDirectory}/SQL/Init.sh:/docker-entrypoint-initdb.d/init.sh"
    };

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        PostgresDatabaseConnectionString = _postgres.StartAsync().Result;

        builder.UseSetting("ConnectionStrings:Ecommerce", PostgresDatabaseConnectionString);

        builder.UseEnvironment("Release");
    }
}