namespace Ecommerce.Infrastructure.Persistence;

public interface IDbContext
{
    string GetConnectionString();
}

public class DbContext : IDbContext
{
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Ecommerce") ?? throw new InvalidDataException("Trying to get [ConnectionStrings]");
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }
}
