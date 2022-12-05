namespace Ecommerce.Store.Infrastructure.Environment;

public class ConfigurationSettings
{
    private readonly IConfiguration configuration;

    public ConfigurationSettings(IConfiguration configuration)
    {
        this.configuration = configuration;

        PostgresConnection = this.configuration["Database:Postgres.Ecommerce.Store.Connection"] ?? throw new InvalidDataException();
    }

    public required string PostgresConnection { get; set; }
}
