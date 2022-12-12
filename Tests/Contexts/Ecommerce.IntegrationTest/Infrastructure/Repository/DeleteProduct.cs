namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Dapper;
using Moq;
using Npgsql;

using Common.Fixture.Infrastructure.Database;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

public sealed class DeleteProductIntegrationTest
{
    private PostgresDatabaseFactory _postgres { get; init; }

    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();

    public DeleteProductIntegrationTest()
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
    public async Task GivenProductsInDatabase_WhenDelete_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productId = Guid.Parse("092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        await new ProductRepository(_dbContext).Delete(productId, CancellationToken.None);
    }

    [Test]
    public async Task GivenProductsInDatabase_WhenDelete_ThenThrowNotFoundException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('71a4c1e7-625f-4576-b7a5-188537da5bfe', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productId = Guid.Parse("092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        Assert.ThrowsAsync<ProductNotFoundException>(
            async () => await new ProductRepository(_dbContext).Delete(productId, CancellationToken.None));
    }
}
