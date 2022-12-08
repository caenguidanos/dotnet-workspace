namespace Ecommerce.IntegrationTest.Repository;

using Ecommerce.IntegrationTest.Fixture;

public class GetProducts
{
    private string connectionString = string.Empty;

    private readonly PostgresFixture _postgresFixture = new PostgresFixture();

    [OneTimeSetUp]
    public void Setup()
    {
        connectionString = _postgresFixture.StartServer(port: 9200, database: "ecommerce");
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        _postgresFixture.DisposeServer();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}