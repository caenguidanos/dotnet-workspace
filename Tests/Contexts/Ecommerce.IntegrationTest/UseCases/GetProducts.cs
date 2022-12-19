namespace Ecommerce.IntegrationTest.UseCases;

using System.Net;
using System.Net.Http.Headers;

using Ecommerce.IntegrationTest.App;
using Ecommerce.IntegrationTest.Util;

public class GetProductsIntegrationTest
{
    private HttpClient _httpClient;
    private WebAppFactory _app;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _app = new WebAppFactory();
        await _app.StartDatabaseAsync();
        _httpClient = _app.CreateClient();
    }

    [Test]
    public async Task GivenNoProductsOnDatabase_WhenRequestAll_ThenReturnEmptyCollection()
    {
        await _app.ExecuteSqlAsync("""
            TRUNCATE product;
        """);

        var response = await _httpClient.GetAsync("/product");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType, Is.EqualTo(MediaTypeHeaderValue.Parse("application/json")));

        string responseBody = await response.Content.ReadAsStringAsync();
        string responseBodySnapshot = """
            []
        """;

        Assert.That(responseBody, Is.EqualTo(JsonUtil.CleanString(responseBodySnapshot)));
    }

    [Test]
    public async Task GivenProductsOnDatabase_WhenRequestAll_ThenReturnCollection()
    {
        await _app.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status, created_at, updated_at)
            VALUES (
                '092cc0ea-a54f-48a3-87ed-0e7f43c023f1',
                'American Professional II Stratocaster',
                'Great guitar',
                219900,
                1,
                '2022-12-18T21:25:30.043264Z',
                '2022-12-18T21:25:30.043264Z'
            );

            INSERT INTO product (id, title, description, price, status, created_at, updated_at)
            VALUES (
                '8a5b3e4a-3e08-492c-869e-317a4d04616a',
                'Mustang Shelby GT500',
                'Great car',
                7900000,
                1,
                '2022-12-18T21:25:30.043264Z',
                '2022-12-18T21:25:30.043264Z'
            );
        """);

        var response = await _httpClient.GetAsync("/product");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType, Is.EqualTo(MediaTypeHeaderValue.Parse("application/json")));

        string responseBody = await response.Content.ReadAsStringAsync();
        string responseBodySnapshot = """
            [
                {
                    "id": "092cc0ea-a54f-48a3-87ed-0e7f43c023f1",
                    "title": "American Professional II Stratocaster",
                    "description": "Great guitar",
                    "price": 219900,
                    "status": 1,
                    "created_at": "2022-12-18T21:25:30.043264Z",
                    "updated_at": "2022-12-18T21:25:30.043264Z"
                },
                {
                    "id": "8a5b3e4a-3e08-492c-869e-317a4d04616a",
                    "title": "Mustang Shelby GT500",
                    "description": "Great car",
                    "price": 7900000,
                    "status": 1,
                    "created_at": "2022-12-18T21:25:30.043264Z",
                    "updated_at": "2022-12-18T21:25:30.043264Z"
                }
            ]
        """;

        Assert.That(responseBody, Is.EqualTo(JsonUtil.CleanString(responseBodySnapshot)));
    }
}