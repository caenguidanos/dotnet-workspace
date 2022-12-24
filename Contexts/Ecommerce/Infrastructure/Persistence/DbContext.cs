namespace Ecommerce.Infrastructure;

public interface IDbContext
{
    string GetConnectionString();
}

public sealed class DbContext : IDbContext
{
    private string _connectionString { get; }

    public DbContext(IConfiguration configuration)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        _connectionString = configuration.GetConnectionString("Ecommerce") ??
                            throw new InvalidDataException("Trying to get [ConnectionStrings]");
    }

    public string GetConnectionString() => _connectionString;
}