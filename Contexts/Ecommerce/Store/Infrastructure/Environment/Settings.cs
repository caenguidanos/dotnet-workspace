namespace Ecommerce.Store.Infrastructure.Environment;

public class ConfigurationSettings
{
    public ConfigurationSettings(IConfiguration configuration)
    {
        PostgresConnection = configuration["Databases:Postgres:ConnectionString:Ecommerce:Store"]
            ?? throw new InvalidDataException("Trying to get [Databases:Postgres:ConnectionString:Ecommerce:Store]");
    }

    public required string PostgresConnection { get; set; }
}
