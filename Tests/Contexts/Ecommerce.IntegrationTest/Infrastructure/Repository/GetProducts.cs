namespace Ecommerce.IntegrationTest.Infrastructure.Repository;

using Ecommerce.IntegrationTest.Fixture;

public class GetProducts
{
    private string _connectionString = string.Empty;
    private readonly PostgresFixture _postgresFixture;

    public GetProducts()
    {
        _postgresFixture = new PostgresFixture();
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _connectionString = _postgresFixture.StartServer(
            port: 9200,
            database: "ecommerce");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _postgresFixture.DisposeServer();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
