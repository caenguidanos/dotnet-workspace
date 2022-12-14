namespace Ecommerce.Infrastructure.Persistence;

public interface IDbSeed
{
    public Task PopulateAsync();
}

public sealed class DbSeed : IDbSeed
{
    private IDbContext _dbContext { get; }

    public DbSeed(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task PopulateAsync()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());

        // Ensure deleted database table data
        await conn.ExecuteAsync(@"TRUNCATE product");

        // Add new data to ecommerce.product table
        await conn.ExecuteAsync(@"
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');

            INSERT INTO product (id, title, description, price, status)
            VALUES ('8a5b3e4a-3e08-492c-869e-317a4d04616a', 'Mustang Shelby GT500', 'Great car', 7900000, 'published');

            INSERT INTO product (id, title, description, price, status)
            VALUES ('71a4c1e7-625f-4576-b7a5-188537da5bfe', 'Antelope Orion +32', 'Great audio interface', 300000, 'draft');
        ");
    }
}