namespace Ecommerce.IntegrationTest.UseCases;

using System.Net;
using System.Net.Http.Headers;

using Ecommerce.IntegrationTest.Util;

public class RemoveProductByIdIntegrationTest : IClassFixture<EcommerceWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly string _connectionString;

    public RemoveProductByIdIntegrationTest(EcommerceWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        _connectionString = factory.PostgresDatabaseConnectionString;
    }

    [Fact]
    public async Task GivenNoProductsOnDatabase_WhenRequestById_ThenReturnProblemDetails()
    {
        await PostgresUtil.ExecuteAsync(_connectionString, """
            TRUNCATE product;
        """);

        var response = await _client.DeleteAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        Assert.Equal(response.Content.Headers.ContentType, MediaTypeHeaderValue.Parse("application/problem+json"));

        var responseBody = await response.Content.ReadAsStringAsync();
        var responseBodySnapshot = """
            {
                "title": "NotFound",
                "status": 404,
                "detail": "Product not found with criteria",
                "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
            }
        """;

        Assert.Equal(responseBody, JsonUtil.CleanString(responseBodySnapshot));
    }

    [Fact]
    public async Task GivenProductsOnDatabase_WhenRequestById_ThenReturnAck()
    {
        await PostgresUtil.ExecuteAsync(_connectionString, """
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);

            INSERT INTO product (id, title, description, price, status)
            VALUES ('8a5b3e4a-3e08-492c-869e-317a4d04616a', 'Mustang Shelby GT500', 'Great car', 7900000, 1);
        """);

        var response = await _client.DeleteAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a");

        Assert.Equal(response.StatusCode, HttpStatusCode.Accepted);
        Assert.Equal(response.Content.Headers.ContentType, MediaTypeHeaderValue.Parse("application/json"));

        var responseBody = await response.Content.ReadAsStringAsync();
        var responseBodySnapshot = """
            {
                "id": "8a5b3e4a-3e08-492c-869e-317a4d04616a"
            }
        """;

        Assert.Equal(responseBody, JsonUtil.CleanString(responseBodySnapshot));

        var responsePostRemove = await _client.GetAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a");

        Assert.Equal(responsePostRemove.StatusCode, HttpStatusCode.NotFound);
    }
}