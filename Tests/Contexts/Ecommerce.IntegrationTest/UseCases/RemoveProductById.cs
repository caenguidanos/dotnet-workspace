namespace Ecommerce.IntegrationTest.UseCases;

using System.Net;
using System.Net.Http.Headers;

using Ecommerce.IntegrationTest.App;
using Ecommerce.IntegrationTest.Util;

public class RemoveProductByIdIntegrationTest
{
    private HttpClient _http;
    private WebAppFactory _app;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _app = new WebAppFactory();
        await _app.StartDatabaseAsync();
        _http = _app.CreateClient();
    }

    [Test]
    public async Task GivenNoProductsOnDatabase_WhenRequestById_ThenReturnProblemDetails()
    {
        await _app.ExecuteSqlAsync("""
            TRUNCATE product;
        """);

        var response = await _http.DeleteAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(response.Content.Headers.ContentType, Is.EqualTo(MediaTypeHeaderValue.Parse("application/problem+json")));

        string responseBody = await response.Content.ReadAsStringAsync();
        string responseBodySnapshot = """
            {
                "title": "NotFound",
                "status": 404,
                "detail": "Product not found with criteria",
                "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
            }
        """;

        Assert.That(responseBody, Is.EqualTo(JsonUtil.CleanString(responseBodySnapshot)));
    }

    [Test]
    public async Task GivenProductsOnDatabase_WhenRequestById_ThenReturnAck()
    {
        await _app.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);

            INSERT INTO product (id, title, description, price, status)
            VALUES ('8a5b3e4a-3e08-492c-869e-317a4d04616a', 'Mustang Shelby GT500', 'Great car', 7900000, 1);
        """);

        var response = await _http.DeleteAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
        Assert.That(response.Content.Headers.ContentType, Is.EqualTo(MediaTypeHeaderValue.Parse("text/plain")));

        string responseBody = await response.Content.ReadAsStringAsync();
        string responseBodySnapshot = """
            Accepted
        """;

        Assert.That(responseBody, Is.EqualTo(JsonUtil.CleanString(responseBodySnapshot)));

        var responsePostRemove = await _http.GetAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a");

        Assert.That(responsePostRemove.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}