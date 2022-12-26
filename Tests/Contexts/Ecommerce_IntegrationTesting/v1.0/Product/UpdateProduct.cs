namespace Ecommerce_IntegrationTesting.Product;

[Category("v1.0")]
public sealed class UpdateProductIntegrationTest
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
    public async Task GivenNoProductsOnDatabase_WhenUpdateProduct_ThenReturnNotFound()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;
        """);

        var requestPayload = new StringContent("""
            {
                "title": "Samsung TV 57"
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a", requestPayload);
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
                "instance": "/product/8a5b3e4a-3e08-492c-869e-317a4d04616a"
            }
        """)));
    }

    [Test]
    public async Task GivenProductOnDatabase_WhenUpdateTitleProduct_ThenReturnAccepted()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var requestPayload = new StringContent("""
            {
                "title": "Samsung TV 57"
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        var responseAfterUpdateBody = await responseAfterUpdate.Content.ReadAsStringAsync();
        Assert.That(responseAfterUpdateBody, Is.EqualTo(Json.MinifyString("""
            {
                "id": "092cc0ea-a54f-48a3-87ed-0e7f43c023f1",
                "title": "Samsung TV 57",
                "description": "Great guitar",
                "price": 219900,
                "status": "published"
            }
        """)));
    }

    [Test]
    public async Task GivenProductOnDatabase_WhenUpdateTitleInvalidProduct_ThenReturnBadRequest()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var requestPayload = new StringContent("""
            {
                "title": "j"
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
            {
                "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "title": "BadRequest",
                "status": 400,
                "detail": "Product title is invalid",
                "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
            }
        """)));
    }

    [Test]
    public async Task GivenProductOnDatabase_WhenUpdateDescriptionProduct_ThenReturnAccepted()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var requestPayload = new StringContent("""
            {
                "description": "Great guitar, sure"
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        var responseAfterUpdateBody = await responseAfterUpdate.Content.ReadAsStringAsync();
        Assert.That(responseAfterUpdateBody, Is.EqualTo(Json.MinifyString("""
            {
                "id": "092cc0ea-a54f-48a3-87ed-0e7f43c023f1",
                "title": "American Professional II Stratocaster",
                "description": "Great guitar, sure",
                "price": 219900,
                "status": "published"
            }
        """)));
    }

    [Test]
    public async Task GivenProductOnDatabase_WhenUpdateDescriptionInvalidProduct_ThenReturnBadRequest()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var requestPayload = new StringContent("""
            {
                "description": "o"
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
            {
                "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "title": "BadRequest",
                "status": 400,
                "detail": "Product description is invalid",
                "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
            }
        """)));
    }

    [Test]
    public async Task GivenProductOnDatabase_WhenUpdatePriceProduct_ThenReturnAccepted()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var requestPayload = new StringContent("""
            {
                "price": 500
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        var responseAfterUpdateBody = await responseAfterUpdate.Content.ReadAsStringAsync();
        Assert.That(responseAfterUpdateBody, Is.EqualTo(Json.MinifyString("""
            {
                "id": "092cc0ea-a54f-48a3-87ed-0e7f43c023f1",
                "title": "American Professional II Stratocaster",
                "description": "Great guitar",
                "price": 500,
                "status": "published"
            }
        """)));
    }

    [Test]
    public async Task GivenProductOnDatabase_WhenUpdatePriceInvalidProduct_ThenReturnBadRequest()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var requestPayload = new StringContent("""
            {
                "price": 2
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
            {
                "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "title": "BadRequest",
                "status": 400,
                "detail": "Product price is out of range",
                "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
            }
        """)));
    }

    [Test]
    public async Task GivenProductOnDatabase_WhenUpdateStatusProduct_ThenReturnAccepted()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var requestPayload = new StringContent("""
            {
                "status": "closed"
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        var responseAfterUpdateBody = await responseAfterUpdate.Content.ReadAsStringAsync();
        Assert.That(responseAfterUpdateBody, Is.EqualTo(Json.MinifyString("""
            {
                "id": "092cc0ea-a54f-48a3-87ed-0e7f43c023f1",
                "title": "American Professional II Stratocaster",
                "description": "Great guitar",
                "price": 219900,
                "status": "closed"
            }
        """)));
    }

    [Test]
    public async Task GivenProductOnDatabase_WhenUpdateStatusInvalidProduct_ThenReturnBadRequest()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var requestPayload = new StringContent("""
            {
                "status": "yug87gu"
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
            {
                "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "title": "BadRequest",
                "status": 400,
                "detail": "Product status is invalid",
                "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
            }
        """)));
    }

    [Test]
    public async Task GivenProductOnDatabase_WhenUpdateMultipleProductFields_ThenReturnAccepted()
    {
        using var httpClient = _server.CreateClient();
        httpClient.DefaultRequestHeaders.Add("X-Api-Version", Version);

        await _server.ExecuteSqlAsync("""
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var requestPayload = new StringContent("""
            {
                "title": "American Professional IV Stratocaster",
                "price": 269900,
                "status": "closed"
            }
        """, new MediaTypeHeaderValue("application/json", "utf-8"));

        var response = await httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", requestPayload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");

        var responseAfterUpdateBody = await responseAfterUpdate.Content.ReadAsStringAsync();
        Assert.That(responseAfterUpdateBody, Is.EqualTo(Json.MinifyString("""
            {
                "id": "092cc0ea-a54f-48a3-87ed-0e7f43c023f1",
                "title": "American Professional IV Stratocaster",
                "description": "Great guitar",
                "price": 269900,
                "status": "closed"
            }
        """)));
    }
}