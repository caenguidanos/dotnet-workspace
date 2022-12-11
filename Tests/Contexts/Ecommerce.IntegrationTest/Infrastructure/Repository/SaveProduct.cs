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
public class SaveProductTest
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

        var newProductId = Product.NewID();
        var newProduct = new Product(
            new ProductId(newProductId),
            new ProductTitle("Super title 1"),
            new ProductDescription("Super description 1"),
            new ProductStatus(ProductStatusValue.Published),
            new ProductPrice(200)
        );

        await productRepository.Save(newProduct, CancellationToken.None);

        var product = await productRepository.GetById(newProductId, CancellationToken.None);
        Assert.That(product.Id, Is.EqualTo(newProductId));
        Assert.That(product.Title, Is.EqualTo(newProduct.Title));
        Assert.That(product.Description, Is.EqualTo(newProduct.Description));
        Assert.That(product.Status, Is.EqualTo(newProduct.Status));
        Assert.That(product.Price, Is.EqualTo(newProduct.Price));
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

        var productId = Guid.Parse("71a4c1e7-625f-4576-b7a5-188537da5bfe");
        var product = new Product(
            new ProductId(productId),
            new ProductTitle("Super title 1"),
            new ProductDescription("Super description 1"),
            new ProductStatus(ProductStatusValue.Published),
            new ProductPrice(200)
        );

        Assert.ThrowsAsync<ProductPersistenceException>(async () => await productRepository.Save(product, CancellationToken.None));
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

        var product = new Product(
            new ProductId(Product.NewID()),
            new ProductTitle("Super title 1"),
            new ProductDescription("Super description 1"),
            new ProductStatus(ProductStatusValue.Published),
            new ProductPrice(200)
        );

        Assert.ThrowsAsync<ProductPersistenceException>(async () => await productRepository.Save(product, CancellationToken.None));
    }
}
