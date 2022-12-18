namespace Ecommerce.IntegrationTest.UseCases;

using System.Net;
using System.Net.Http.Headers;

using Ecommerce.IntegrationTest.Util;

public class GetProductByIdIntegrationTest : IClassFixture<EcommerceWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly string _connectionString;

    public GetProductByIdIntegrationTest(EcommerceWebApplicationFactory<Program> factory)
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

        var response = await _client.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

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
    public async Task GivenProductsOnDatabase_WhenRequestById_ThenReturnCoincidence()
    {
        await PostgresUtil.ExecuteAsync(_connectionString, """
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

        var response = await _client.GetAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a");

        Assert.Equal(response.StatusCode, HttpStatusCode.OK);
        Assert.Equal(response.Content.Headers.ContentType, MediaTypeHeaderValue.Parse("application/json"));

        var responseBody = await response.Content.ReadAsStringAsync();
        var responseBodySnapshot = """
            {
                "id": "8a5b3e4a-3e08-492c-869e-317a4d04616a",
                "title": "Mustang Shelby GT500",
                "description": "Great car",
                "price": 7900000,
                "status": 1,
                "created_at": "2022-12-18T21:25:30.043264Z",
                "updated_at": "2022-12-18T21:25:30.043264Z"
            }
        """;

        Assert.Equal(responseBody, JsonUtil.CleanString(responseBodySnapshot));
    }
}