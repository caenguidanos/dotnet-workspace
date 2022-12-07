namespace Ecommerce.Store.Infrastructure.Environment;

public class EnvironmentSettings
{
    public EnvironmentSettings(IConfiguration configuration)
    {
        PostgresConnection = configuration["Databases:Postgres:ConnectionString:Ecommerce:Store"]
            ?? throw new InvalidDataException("Trying to get [Databases:Postgres:ConnectionString:Ecommerce:Store]");
    }

    public string PostgresConnection { get; private set; }
}
