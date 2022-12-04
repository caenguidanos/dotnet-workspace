namespace api.Contexts.Ecommerce.Store.Infrastructure.Environment
{
    public class ConfigurationSettings
    {
        private readonly IConfiguration _configuration;

        public required string PostgresConnection { get; set; }

        public ConfigurationSettings(IConfiguration configuration)
        {
            _configuration = configuration;

            PostgresConnection = _configuration["Database:PostgresEcommerceStoreConnection"] ?? throw new InvalidDataException();
        }
    }
}