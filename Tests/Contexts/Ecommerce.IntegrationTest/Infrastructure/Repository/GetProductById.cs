namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Dapper;
using Moq;
using Npgsql;

using Common.Fixture.Application.Tests;
using Common.Fixture.Infrastructure.Database;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.ValueObject;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

[Category(TestCategory.Integration)]
public sealed class GetProductByIdTest
{
    private PostgresDatabase _postgresDatabase { get; init; }

    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();

    public GetProductByIdTest()
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
    public async Task GivenProductsInDatabase_WhenGetById_ThenReturnCoincidence()
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

        var currentProduct = new Product
        {
            Id = new ProductId(productId),
            Title = new ProductTitle("American Professional II Stratocaster"),
            Description = new ProductDescription("Great guitar"),
            Status = new ProductStatus(ProductStatusValue.Published),
            Price = new ProductPrice(219900)
        };

        var retrievedProduct = await productRepository.GetById(productId, CancellationToken.None);

        Assert.That(currentProduct.ShallowEqual(retrievedProduct), Is.True);
        Assert.That(retrievedProduct.created_at, Is.Not.EqualTo(default(DateTime)));
        Assert.That(retrievedProduct.updated_at, Is.Not.EqualTo(default(DateTime)));
    }

    [Test, Order(2)]
    public async Task GivenProductsInDatabase_WhenGetById_ThenThrowNotFoundException()
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

        Assert.ThrowsAsync<ProductNotFoundException>(async () => await productRepository.GetById(productId, CancellationToken.None));
    }

    [Test]
    public async Task GivenNoTableInDatabase_WhenGetById_ThenThrowsException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            DROP TABLE public.product;
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        Assert.ThrowsAsync<ProductPersistenceException>(async () => await productRepository.GetById(Guid.NewGuid(), CancellationToken.None));
    }
}
