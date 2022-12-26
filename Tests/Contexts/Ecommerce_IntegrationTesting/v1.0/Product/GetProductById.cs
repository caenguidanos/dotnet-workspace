namespace Ecommerce_IntegrationTesting.Product;

[Category("v1.0")]
public sealed class GetProductByIdIntegrationTest
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
    public async Task GivenNoProductsOnDatabase_WhenRequestById_ThenReturnProblemDetails()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;
        """);

        var response = await httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));
        });

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
            {
                "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                "title": "NotFound",
                "status": 404,
                "detail": "Product not found",
                "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
            }
        """)));
    }

    [Test]
    public async Task GivenProductsOnDatabase_WhenRequestById_ThenReturnCoincidence()
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

        var response = await httpClient.GetAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a");
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/json", "utf-8")));
        });

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
            {
                "id": "8a5b3e4a-3e08-492c-869e-317a4d04616a",
                "title": "Mustang Shelby GT500",
                "description": "Great car",
                "price": 7900000,
                "status": "published"
            }
        """)));
    }
}