namespace Ecommerce_IntegrationTesting.Product;

[Category("v1.0")]
public sealed class GetProductsIntegrationTest
{
    private const string Version = "1.0";

    private EcommerceWebApplicationFactory _server = Mock.Of<EcommerceWebApplicationFactory>();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _server = new EcommerceWebApplicationFactory();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _server.DropDatabaseAsync();
    }

    [Test]
    public async Task GivenNoProductsOnDatabase_WhenRequestAll_ThenReturnEmptyCollection()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;
        """);

        var response = await httpClient.GetAsync("/product");
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/json", "utf-8")));
        });

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.That(responseBody, Is.EqualTo(Json.MinifyString("[]")));
    }

    [Test]
    public async Task GivenProductsOnDatabase_WhenRequestAll_ThenReturnCollection()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');

            INSERT INTO product (id, title, description, price, status)
            VALUES ('8a5b3e4a-3e08-492c-869e-317a4d04616a', 'Mustang Shelby GT500', 'Great car', 7900000, 'published');
        """);

        var response = await httpClient.GetAsync("/product");
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/json", "utf-8")));
        });

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
            [
                {
                    "id": "092cc0ea-a54f-48a3-87ed-0e7f43c023f1",
                    "title": "American Professional II Stratocaster",
                    "description": "Great guitar",
                    "price": 219900,
                    "status": "published"
                },
                {
                    "id": "8a5b3e4a-3e08-492c-869e-317a4d04616a",
                    "title": "Mustang Shelby GT500",
                    "description": "Great car",
                    "price": 7900000,
                    "status": "published"
                }
            ]
        """)));
    }
}