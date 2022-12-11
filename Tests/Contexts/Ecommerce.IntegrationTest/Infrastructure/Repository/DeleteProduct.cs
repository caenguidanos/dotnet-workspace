namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Dapper;
using Moq;
using Npgsql;

using Common.Fixture.Infrastructure.Database;
using Common.Fixture.Application.Tests;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

[Category(TestCategory.Integration)]
public class DeleteProductTest
{
    private PostgresDatabase _postgresDatabase { get; init; }

    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();

    public DeleteProductTest()
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
    public async Task GivenProductsInDatabase_WhenDelete_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE public.product;

            INSERT INTO public.product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        var productId = Guid.Parse("092cc0ea-a54f-48a3-87ed-0e7f43c023f1");
        await productRepository.Delete(productId, CancellationToken.None);
    }

    [Test, Order(2)]
    public async Task GivenProductsInDatabase_WhenDelete_ThenThrowNotFoundException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE public.product;

            INSERT INTO public.product (id, title, description, price, status)
            VALUES ('71a4c1e7-625f-4576-b7a5-188537da5bfe', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        var productId = Guid.Parse("092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        Assert.ThrowsAsync<ProductNotFoundException>(async () => await productRepository.Delete(productId, CancellationToken.None));
    }

    [Test]
    public async Task GivenNoTableInDatabase_WhenDelete_ThenThrowsException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            DROP TABLE public.product;
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        Assert.ThrowsAsync<ProductPersistenceException>(async () => await productRepository.Delete(Guid.NewGuid(), CancellationToken.None));
    }
}
