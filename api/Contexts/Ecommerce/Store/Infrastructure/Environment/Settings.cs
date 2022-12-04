namespace api.Contexts.Ecommerce.Store.Infrastructure.Environment
{
    public class ConfigurationSettings
    {
        private readonly IConfiguration _configuration;

        public required string FaunadbEcommerceStoreSecret { get; set; }

        public ConfigurationSettings(IConfiguration configuration)
        {
            _configuration = configuration;

            FaunadbEcommerceStoreSecret = _configuration["Database:FaunadbEcommerceStoreSecret"] ?? throw new InvalidDataException();
        }
    }
}