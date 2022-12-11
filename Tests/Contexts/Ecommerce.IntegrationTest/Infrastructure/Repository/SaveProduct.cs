namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Moq;

using Common.Fixture.Infrastructure.Database;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.ValueObject;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

public sealed class SaveProductIntegrationTest
{
    private PostgresDatabase _postgresDatabase { get; init; }

    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();

    public SaveProductIntegrationTest()
    {
        _postgresDatabase = new PostgresDatabase(
            name: "ecommerce",
            volumes: IntegrationTestConfiguration.containerVolumes);
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        string connectionString = await _postgresDatabase.StartAsync();

        Mock
            .Get(_dbContext)
            .Setup(dbContext => dbContext.GetConnectionString())
            .Returns(connectionString);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _postgresDatabase.DisposeAsync();
    }

    [Test]
    public async Task GivenProduct_WhenSave_ThenPass()
    {
        var product = new Product
        {
            Id = new ProductId(Common.Domain.Schema.NewID()),
            Title = new ProductTitle("Super title 1"),
            Description = new ProductDescription("Super description 1"),
            Status = new ProductStatus(ProductStatusValue.Published),
            Price = new ProductPrice(200)
        };

        await new ProductRepository(_dbContext).Save(product, CancellationToken.None);
    }
}
