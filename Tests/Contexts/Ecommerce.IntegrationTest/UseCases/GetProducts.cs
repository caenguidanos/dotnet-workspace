namespace Ecommerce.IntegrationTest.UseCases;

using Moq;
using System.Net;
using System.Net.Http.Headers;
using Ecommerce.IntegrationTest.App;
using Ecommerce.IntegrationTest.Util;

public sealed class GetProductsIntegrationTest
{
    private HttpClient _http = Mock.Of<HttpClient>();
    private WebAppFactory _server = Mock.Of<WebAppFactory>();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _server = new WebAppFactory();
        _http = _server.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _server.Dispose();
        _http.Dispose();
    }

    [Test]
    public async Task GivenNoProductsOnDatabase_WhenRequestAll_ThenReturnEmptyCollection()
    {
        await _server.ExecuteSqlAsync("""
            TRUNCATE product;
        """);

        var response = await _http.GetAsync("/product");

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(MediaTypeHeaderValue.Parse("application/json")));
        });

        var responseBody = await response.Content.ReadAsStringAsync();
        const string responseBodySnapshot = "[]";

        Assert.That(responseBody, Is.EqualTo(JsonUtil.MinifyString(responseBodySnapshot)));
    }

    [Test]
    public async Task GivenProductsOnDatabase_WhenRequestAll_ThenReturnCollection()
    {
        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status, __created_at__, __updated_at__)
            VALUES (
                '092cc0ea-a54f-48a3-87ed-0e7f43c023f1',
                'American Professional II Stratocaster',
                'Great guitar',
                219900,
                1,
                '2022-12-18T21:25:30.043264Z',
                '2022-12-18T21:25:30.043264Z'
            );

            INSERT INTO product (id, title, description, price, status, __created_at__, __updated_at__)
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

        var response = await _http.GetAsync("/product");

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(MediaTypeHeaderValue.Parse("application/json")));
        });

        var responseBody = await response.Content.ReadAsStringAsync();
        const string responseBodySnapshot = """
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

        Assert.That(responseBody, Is.EqualTo(JsonUtil.MinifyString(responseBodySnapshot)));
    }
}