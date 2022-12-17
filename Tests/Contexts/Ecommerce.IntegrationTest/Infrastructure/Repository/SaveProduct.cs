namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Moq;
using Npgsql;
using Dapper;

using Common.Fixture.Infrastructure.Database;

using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.ValueObject;
using Ecommerce.Domain.Model;

public sealed class SaveProductIntegrationTest
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
    public async Task GivenProduct_WhenSave_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE product;
        ";

        await conn.ExecuteAsync(sql);

        var product = new Product
        {
            Id = new ProductId(),
            Title = new ProductTitle("Super title 1"),
            Description = new ProductDescription("Super description 1"),
            Status = new ProductStatus(ProductStatusValue.Published),
            Price = new ProductPrice(200)
        };

        var productIntegrity = product.CheckIntegrity();
        Assert.That(productIntegrity.IsFaulted, Is.False);

        var saveProductResult = await new ProductRepository(_dbContext).Save(product, CancellationToken.None);
        Assert.That(saveProductResult.IsFaulted, Is.False);
    }
}
