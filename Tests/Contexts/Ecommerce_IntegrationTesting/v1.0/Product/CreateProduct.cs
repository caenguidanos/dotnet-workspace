namespace Ecommerce_IntegrationTesting.Product;

[Category("v1.0")]
public sealed class CreateProductIntegrationTest
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
    public async Task GivenNoProductsOnDatabase_WhenCreateProduct_ThenReturnAcceptedAndCheckExistence()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;
        """);

        var requestPayload = new StringContent("""
            {
                "id": "8a5b3e4a-3e08-492c-869e-317a4d04616a",
                "title": "Samsung TV 55",
                "description": "Perfect for movies",
                "price": 70000,
                "status": "closed"
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PostAsync("/product", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterCreate = await httpClient.GetAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a");
        Assert.That(responseAfterCreate.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GivenProductsOnDatabase_WhenCreateInvalidProduct_ThenReturnBadRequest()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('256a4889-bd23-436c-93ac-5ec4100abceb', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var testCases = new List<TestCaseBadRequestWithSnapshot>
        {
            new()
            {
                In = """
                    {
                        "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                        "title": "Samsung TV 55",
                        "description": "Perfect for movies",
                        "price": 70000,
                        "status": "published"
                    }
                """,
                Out = """
                    {
                        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        "title": "BadRequest",
                        "status": 400,
                        "detail": "Product id is not unique",
                        "instance": "/product"
                    }
                """
            },
            new()
            {
                In = """
                    {
                        "id": "8a5b3e4a-3e08",
                        "title": "Samsung TV 55",
                        "description": "Perfect for movies",
                        "price": 70000,
                        "status": "closed"
                    }
                """,
                Out = """
                    {
                      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                      "title": "Bad Request",
                      "status": 400
                    }
                """
            },
            new()
            {
                In = """
                    {
                        "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                        "title": "a",
                        "description": "Perfect for movies",
                        "price": 70000,
                        "status": "closed"
                    }
                """,
                Out = """
                    {
                        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        "title": "BadRequest",
                        "status": 400,
                        "detail": "Product title is invalid",
                        "instance": "/product"
                    }
                """
            },
            new()
            {
                In = """
                    {
                        "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                        "title": "Samsung TV 55",
                        "description": "e",
                        "price": 70000,
                        "status": "closed"
                    }
                """,
                Out = """
                    {
                        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        "title": "BadRequest",
                        "status": 400,
                        "detail": "Product description is invalid",
                        "instance": "/product"
                    }
                """
            },
            new()
            {
                In = """
                    {
                        "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                        "title": "Samsung TV 55",
                        "description": "Perfect for movies",
                        "price": 1,
                        "status": "closed"
                    }
                """,
                Out = """
                    {
                        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        "title": "BadRequest",
                        "status": 400,
                        "detail": "Product price is out of range",
                        "instance": "/product"
                    }
                """
            },
            new()
            {
                In = """
                    {
                        "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                        "title": "Samsung TV 55",
                        "description": "Perfect for movies",
                        "price": 2000,
                        "status": "hello"
                    }
                """,
                Out = """
                    {
                        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        "title": "BadRequest",
                        "status": 400,
                        "detail": "Product status is invalid",
                        "instance": "/product"
                    }
                """
            }
        };

        foreach (var testCase in testCases)
        {
            var response = await httpClient.PostAsync("/product",
                new StringContent(testCase.In, new MediaTypeHeaderValue("application/json", "utf-8")));

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));
            });

            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.That(responseBody, Is.EqualTo(Json.MinifyString(testCase.Out)));
        }
    }
}

file sealed class TestCaseBadRequestWithSnapshot
{
    public required string In { get; init; }
    public required string Out { get; init; }
}