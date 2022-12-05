namespace Ecommerce.Store.Infrastructure.Environment;

public class ConfigurationSettings
{
    public ConfigurationSettings(IConfiguration configuration)
    {
        PostgresConnection = configuration["Database:Postgres.Connection.Ecommerce.Store"] ?? throw new InvalidDataException();
    }

    public required string PostgresConnection { get; set; }
}
