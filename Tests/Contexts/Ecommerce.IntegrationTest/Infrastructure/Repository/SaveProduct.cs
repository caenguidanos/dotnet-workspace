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
public sealed class SaveProductTest
{
    private PostgresDatabase _postgresDatabase { get; init; }

    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();

    public SaveProductTest()
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
    public async Task GivenValidProduct_WhenSave_ThenPass()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            TRUNCATE public.product;
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        var newProductId = Common.Domain.Schema.NewID();
        var newProduct = new Product
        {
            Id = new ProductId(newProductId),
            Title = new ProductTitle("Super title 1"),
            Description = new ProductDescription("Super description 1"),
            Status = new ProductStatus(ProductStatusValue.Published),
            Price = new ProductPrice(200)
        };

        await productRepository.Save(newProduct, CancellationToken.None);

        var product = await productRepository.GetById(newProductId, CancellationToken.None);

        Assert.That(newProduct.ShallowEqual(product), Is.True);
    }

    [Test, Order(2)]
    public async Task GivenProductWithSameId_WhenSave_ThenThrowException()
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

        var product = new Product
        {
            Id = new ProductId(Guid.Parse("71a4c1e7-625f-4576-b7a5-188537da5bfe")),
            Title = new ProductTitle("Super title 1"),
            Description = new ProductDescription("Super description 1"),
            Status = new ProductStatus(ProductStatusValue.Published),
            Price = new ProductPrice(200)
        };

        Assert.ThrowsAsync<ProductPersistenceException>(async () => await productRepository.Save(product, CancellationToken.None));
    }

    public async Task GivenProductWithSameTitle_WhenSave_ThenThrowException()
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

        var product = new Product
        {
            Id = new ProductId(Product.NewID()),
            Title = new ProductTitle("merican Professional II Stratocaster"),
            Description = new ProductDescription("Super description 1"),
            Status = new ProductStatus(ProductStatusValue.Published),
            Price = new ProductPrice(200)
        };

        Assert.ThrowsAsync<ProductTitleUniqueException>(async () => await productRepository.Save(product, CancellationToken.None));
    }

    [Test]
    public async Task GivenNoTableInDatabase_WhenSave_ThenThrowsException()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();

        string sql = @"
            DROP TABLE public.product;
        ";

        await conn.ExecuteAsync(sql).ConfigureAwait(false);

        var productRepository = new ProductRepository(_dbContext);

        var product = new Product
        {
            Id = new ProductId(Common.Domain.Schema.NewID()),
            Title = new ProductTitle("Super title 1"),
            Description = new ProductDescription("Super description 1"),
            Status = new ProductStatus(ProductStatusValue.Published),
            Price = new ProductPrice(200)
        };

        Assert.ThrowsAsync<ProductPersistenceException>(async () => await productRepository.Save(product, CancellationToken.None));
    }
}
