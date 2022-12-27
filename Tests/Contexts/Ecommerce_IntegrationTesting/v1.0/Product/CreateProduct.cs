namespace Ecommerce_IntegrationTesting.Product;

[Category("v1.0")]
public sealed class CreateProductIntegrationTest
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

    ///  <summary>
    ///     Given a valid Product payload as json string,
    ///         when request endpoint and pass all validations,
    ///             then return http-status 202 and check saved entity after new request.
    /// </summary>
    [Test]
    public async Task Product_Validation()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "8a5b3e4a-3e08-492c-869e-317a4d04616a",
                    "title": "Samsung TV 55",
                    "description": "Perfect for movies",
                    "price": 70000,
                    "status": "draft"
                }
             """, new MediaTypeHeaderValue("application/json", "utf-8")));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterCreate = await _httpClient.GetAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a");
        Assert.That(responseAfterCreate.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    /// <summary>
    ///     Given a Product payload as json string with invalid Product.Id,
    ///         when request endpoint and validate type on endpoint,
    ///             then return http-status 400 and generic problem details payload.
    /// </summary>
    [Test]
    public async Task ProductId_Validation_I()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "8a5b3e4a-3e08-492c-869e-317a4d04616",
                    "title": "Samsung TV 55",
                    "description": "Perfect for movies",
                    "price": 70000,
                    "status": "draft"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                  "title": "Bad Request",
                  "status": 400
                }
            """)));
        });
    }

    /// <summary>
    ///     Given a Product payload as json string with not unique Product.Id,
    ///         when request endpoint and validate on domain service,
    ///             then return http-status 400 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task ProductId_Validation_II()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('256a4889-bd23-436c-93ac-5ec4100abceb', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                    "title": "Samsung TV 55",
                    "description": "Perfect for movies",
                    "price": 70000,
                    "status": "published"
                }
            """
            , new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "BadRequest",
                    "status": 400,
                    "detail": "Product id is not unique",
                    "instance": "/product"
                }
            """)));
        });
    }

    /// <summary>
    ///     Given a Product payload as json string with invalid Product.Title,
    ///         when request endpoint and validate on domain service,
    ///             then return http-status 400 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task ProductTitle_Validation_I()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                    "title": "a",
                    "description": "Perfect for movies",
                    "price": 70000,
                    "status": "draft"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "BadRequest",
                    "status": 400,
                    "detail": "Product title is invalid",
                    "instance": "/product"
                }
            """)));
        });
    }

    /// <summary>
    ///     Given a Product payload as json string with invalid Product.Title,
    ///         when request endpoint and validate on endpoint,
    ///             then return http-status 400 and generic problem details payload.
    /// </summary>
    [Test]
    public async Task ProductTitle_Validation_II()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                    "title": 89,
                    "description": "Perfect for movies",
                    "price": 70000,
                    "status": "draft"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "Bad Request",
                    "status": 400
                }
            """)));
        });
    }

    /// <summary>
    ///     Given a Product payload as json string with invalid Product.Description,
    ///         when request endpoint and validate on domain service,
    ///             then return http-status 400 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task ProductDescription_Validation_I()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                    "title": "Samsung TV 55",
                    "description": "e",
                    "price": 70000,
                    "status": "draft"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "BadRequest",
                    "status": 400,
                    "detail": "Product description is invalid",
                    "instance": "/product"
                }
            """)));
        });
    }

    /// <summary>
    ///     Given a Product payload as json string with invalid Product.Description,
    ///         when request endpoint and validate on endpoint,
    ///             then return http-status 400 and generic problem details payload.
    /// </summary>
    [Test]
    public async Task ProductDescription_Validation_II()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                    "title": "Samsung TV 55",
                    "description": 899,
                    "price": 70000,
                    "status": "draft"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "Bad Request",
                    "status": 400
                }
            """)));
        });
    }

    /// <summary>
    ///     Given a Product payload as json string with invalid Product.Price,
    ///         when request endpoint and validate on domain service,
    ///             then return http-status 400 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task ProductPrice_Validation_I()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                    "title": "Samsung TV 55",
                    "description": "Perfect for movies",
                    "price": 1,
                    "status": "draft"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "BadRequest",
                    "status": 400,
                    "detail": "Product price is out of range",
                    "instance": "/product"
                }
            """)));
        });
    }

    /// <summary>
    ///     Given a Product payload as json string with invalid Product.Price,
    ///         when request endpoint and validate on endpoint,
    ///             then return http-status 400 and generic problem details payload.
    /// </summary>
    [Test]
    public async Task ProductPrice_Validation_II()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                    "title": "Samsung TV 55",
                    "description": "Perfect for movies",
                    "price": "super price",
                    "status": "draft"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "Bad Request",
                    "status": 400
                }
            """)));
        });
    }

    /// <summary>
    ///     Given a Product payload as json string with invalid Product.Status,
    ///         when request endpoint and validate on domain service,
    ///             then return http-status 400 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task ProductStatus_Validation_I()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                    "title": "Samsung TV 55",
                    "description": "Perfect for movies",
                    "price": 2000,
                    "status": "hello"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "BadRequest",
                    "status": 400,
                    "detail": "Product status is invalid",
                    "instance": "/product"
                }
            """)));
        });
    }

    /// <summary>
    ///     Given a Product payload as json string with invalid Product.Status,
    ///         when request endpoint and validate on endpoint,
    ///             then return http-status 400 and generic problem details payload.
    /// </summary>
    [Test]
    public async Task ProductStatus_Validation_II()
    {
        var response = await _httpClient.PostAsync("/product", new StringContent(
            """
                {
                    "id": "256a4889-bd23-436c-93ac-5ec4100abceb",
                    "title": "Samsung TV 55",
                    "description": "Perfect for movies",
                    "price": 2000,
                    "status": false
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(response.Content.Headers.ContentType, Is.EqualTo(new MediaTypeHeaderValue("application/problem+json")));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "Bad Request",
                    "status": 400
                }
            """)));
        });
    }
}