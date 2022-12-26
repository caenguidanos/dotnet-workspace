namespace Ecommerce_IntegrationTesting.Product;

[Category("v1.0")]
public sealed class GetProductByIdIntegrationTest
{
    private HttpClient _httpClient = Mock.Of<HttpClient>();

    private EcommerceWebApplicationFactory _server = Mock.Of<EcommerceWebApplicationFactory>();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _server = new EcommerceWebApplicationFactory();

        _httpClient = _server.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("X-Api-Version", "1.0");
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _server.DropDatabaseAsync();
    }

    [SetUp]
    public async Task SetUp()
    {
        await _server.TruncateTableAsync(table: "product");
    }

    /// <summary>
    ///     Given no products on the database table,
    ///         when request endpoint with a Product.Id as url-parameter,
    ///             then return http-status 404 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task SearchById_I()
    {
        var response = await _httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    "title": "NotFound",
                    "status": 404,
                    "detail": "Product not found",
                    "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
                }
            """)));
        });
    }

    /// <summary>
    ///     Given products on the database table,
    ///         when request endpoint with a Product.Id as url-parameter,
    ///             then return http-status 200 and the coincidence as json.
    /// </summary>
    [Test]
    public async Task SearchById_II()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');

            INSERT INTO product (id, title, description, price, status)
            VALUES ('8a5b3e4a-3e08-492c-869e-317a4d04616a', 'Mustang Shelby GT500', 'Great car', 7900000, 'published');
        """);

        var response = await _httpClient.GetAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a");
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/json", "utf-8")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "id": "8a5b3e4a-3e08-492c-869e-317a4d04616a",
                    "title": "Mustang Shelby GT500",
                    "description": "Great car",
                    "price": 7900000,
                    "status": "published"
                }
            """)));
        });
    }
}