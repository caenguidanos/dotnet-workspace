using FaunaDB.Client;
using Microsoft.Extensions.Options;

namespace api.Contexts.Ecommerce.Store.Infrastructure.Persistence
{
    public class DatabaseClient
    {
        public readonly FaunaClient client;

        public DatabaseClient(IOptions<DatabaseSettings> databaseSettings)
        {
            client = new FaunaClient(databaseSettings.Value.EcommerceStoreSecret);
        }
    }
}