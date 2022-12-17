namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Moq;
using Npgsql;
using Dapper;

using Common.Fixture.Infrastructure.Database;

using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Domain.Exception;

public sealed class DeleteProductIntegrationTest
{
    private readonly PostgresDatabaseFactory _postgres = new(template: "ecommerce");

    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();

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

        await conn.ExecuteAsync(sql);

        var productId = Guid.Parse("092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        var result = await new ProductRepository(_dbContext).Delete(productId, CancellationToken.None);

        Assert.That(result.IsFaulted, Is.False);
    }

    [Test]
    public async Task GivenProductsInDatabase_WhenDelete_ThenMatchNotFoundException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;
        ";

        await conn.ExecuteAsync(sql);

        var productId = Guid.Parse("092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        var result = await new ProductRepository(_dbContext).Delete(productId, CancellationToken.None);

        Assert.That(result.IsFaulted, Is.True);
        Assert.That(result.Error, Is.InstanceOf<ProductNotFoundException>());
    }
}
