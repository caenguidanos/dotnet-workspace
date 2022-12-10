namespace Ecommerce.Infrastructure.Persistence;

using Dapper;
using Npgsql;

public interface IDbSeed
{
    Task RunAsync();
}

public class DbSeed : IDbSeed
{
    private IDbContext _dbContext { get; init; }

    public DbSeed(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task RunAsync()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());

        string sql = @"
            TRUNCATE public.product;

            INSERT INTO public.product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);

            INSERT INTO public.product (id, title, description, price, status)
            VALUES ('8a5b3e4a-3e08-492c-869e-317a4d04616a', 'Mustang Shelby GT500', 'Great car', 7900000, 1);

            INSERT INTO public.product (id, title, description, price, status)
            VALUES ('71a4c1e7-625f-4576-b7a5-188537da5bfe', 'Antelope Orion +32', 'Great audio interface', 300000, 1);
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);
    }
}
