namespace Ecommerce.Infrastructure.Persistence;

using Microsoft.Extensions.Configuration;

public interface IDbContext
{
    string GetConnectionString();
}

public class DbContext : IDbContext
{
    private string _connectionString { get; init; }

    public DbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Ecommerce") ?? throw new InvalidDataException("Trying to get [ConnectionStrings]");
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }
}
