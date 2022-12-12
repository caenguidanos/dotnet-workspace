namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Dapper;
using Moq;
using Npgsql;

using Common.Fixture.Infrastructure.Database;

using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

public sealed class GetProductsIntegrationTest
{
    private PostgresDatabaseFactory _postgres { get; init; }

    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();

    public GetProductsIntegrationTest()
    {
        _postgres = new PostgresDatabaseFactory(template: "ecommerce");
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        string connectionString = await _postgres.StartAsync();

        Mock
            .Get(_dbContext)
            .Setup(dbContext => dbContext.GetConnectionString())
            .Returns(connectionString);
    }

    [Test]
    public async Task GivenProductsInDatabase_WhenGet_ThenReturnsAll()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);

            INSERT INTO product (id, title, description, price, status)
            VALUES ('8a5b3e4a-3e08-492c-869e-317a4d04616a', 'Mustang Shelby GT500', 'Great car', 7900000, 1);

            INSERT INTO product (id, title, description, price, status)
            VALUES ('71a4c1e7-625f-4576-b7a5-188537da5bfe', 'Antelope Orion +32', 'Great audio interface', 300000, 1);
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        var products = await productRepository.Get(CancellationToken.None);
        Assert.That(products.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task GivenNoProductsInDatabase_WhenGet_ThenReturnsEmptyList()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        await conn.ExecuteAsync("TRUNCATE product").ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        var products = await productRepository.Get(CancellationToken.None);
        Assert.IsEmpty(products);
    }
}
