namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Moq;
using Npgsql;
using Dapper;

using Common.Fixture.Infrastructure.Database;

using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Domain.Exception;
using Ecommerce.Domain.Entity;

public sealed class GetProductByIdIntegrationTest
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
    public async Task GivenProductsInDatabase_WhenGetById_ThenMatchCoincidence()
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

        var result = await new ProductRepository(_dbContext).GetById(productId, CancellationToken.None);

        Assert.That(result.IsFaulted, Is.False);
        Assert.That(result.Value, Is.InstanceOf<Product>());
    }

    [Test]
    public async Task GivenProductsInDatabase_WhenGetById_ThenMatchNotFoundException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('71a4c1e7-625f-4576-b7a5-188537da5bfe', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);
        ";

        await conn.ExecuteAsync(sql);

        var productId = Guid.Parse("092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        var result = await new ProductRepository(_dbContext).GetById(productId, CancellationToken.None);

        Assert.That(result.IsFaulted, Is.True);
        Assert.That(result.Error, Is.InstanceOf<ProductNotFoundException>());
    }
}
