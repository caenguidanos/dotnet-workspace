namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Dapper;
using Moq;
using Npgsql;

using Common.Fixture.Infrastructure.Database;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

public class GetProductsIntegrationTest
{
    private readonly PostgresDatabase _postgresDatabase;

    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();

    public GetProductsIntegrationTest()
    {
        _postgresDatabase = new PostgresDatabase(
            name: "ecommerce",
            volumes: Config.postgresDatabaseVolumes);
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        string connectionString = await _postgresDatabase.StartAsync();

        Mock
            .Get(_dbContext)
            .Setup(dbContext => dbContext
                .GetConnectionString()).Returns(connectionString);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _postgresDatabase.DisposeAsync();
    }

    [Test, Order(1)]
    public async Task GivenProductsInDatabase_WhenGet_ThenReturnsAll()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE public.product;

            INSERT INTO public.product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);

            INSERT INTO public.product (id, title, description, price, status)
            VALUES ('8a5b3e4a-3e08-492c-869e-317a4d04616a', 'Mustang Shelby GT500', 'Great car', 7900000, 1);

            INSERT INTO public.product (id, title, description, price, status)
            VALUES ('71a4c1e7-625f-4576-b7a5-188537da5bfe', 'Antelope Orion +32', 'Great audio interface', 300000, 1);
        ";

        await conn.ExecuteAsync(sql);

        var productRepository = new ProductRepository(_dbContext);

        var products = await productRepository.Get(CancellationToken.None);
        Assert.That(products.Count, Is.EqualTo(3));
    }

    [Test, Order(2)]
    public async Task GivenNoProductsInDatabase_WhenGet_ThenReturnsEmptyList()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE public.product;
        ";

        await conn.ExecuteAsync(sql);

        var productRepository = new ProductRepository(_dbContext);

        var products = await productRepository.Get(CancellationToken.None);
        Assert.IsEmpty(products);
    }

    [Test, Order(3)]
    public async Task GivenCorruptedProductsOnTitleInDatabase_WhenGet_ThenThrowsException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE public.product;

            INSERT INTO public.product (id, title, description, price, status) -- title corrupted
            VALUES ('8ba4e56f-6b2b-49d1-9aeb-16a3a8740511', '', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql);

        var productRepository = new ProductRepository(_dbContext);

        Assert.ThrowsAsync<ProductTitleInvalidException>(
            async () => await productRepository.Get(CancellationToken.None));
    }

    [Test, Order(4)]
    public async Task GivenCorruptedProductsOnDescriptionInDatabase_WhenGet_ThenThrowsException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE public.product;

            INSERT INTO public.product (id, title, description, price, status) -- description corrupted
            VALUES ('954d73fc-30d8-4aa7-bab6-2f5c090fd2dc', 'American Professional II Stratocaster', '', 219900, 1);
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        Assert.ThrowsAsync<ProductDescriptionInvalidException>(
            async () => await productRepository.Get(CancellationToken.None));
    }

    [Test, Order(5)]
    public async Task GivenCorruptedProductsOnPriceInDatabase_WhenGet_ThenThrowsException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE public.product;

            INSERT INTO public.product (id, title, description, price, status) -- price corrupted
            VALUES ('ac381bcb-1b14-497d-86e8-b64c8b6e9d01', 'American Professional II Stratocaster', 'Great guitar', 0, 1);
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        Assert.ThrowsAsync<ProductPriceInvalidException>(
            async () => await productRepository.Get(CancellationToken.None));
    }

    [Test, Order(6)]
    public async Task GivenCorruptedProductsOnStatusInDatabase_WhenGet_ThenThrowsException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE public.product;

            INSERT INTO public.product (id, title, description, price, status) -- status corrupted
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 3);
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        Assert.ThrowsAsync<ProductStatusInvalidException>(async () => await productRepository.Get(CancellationToken.None));
    }

    [Test]
    public async Task GivenNoTableInDatabase_WhenGet_ThenThrowsException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            DROP TABLE public.product;
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        Assert.ThrowsAsync<ProductRepositoryPersistenceException>(async () => await productRepository.Get(CancellationToken.None));
    }
}
