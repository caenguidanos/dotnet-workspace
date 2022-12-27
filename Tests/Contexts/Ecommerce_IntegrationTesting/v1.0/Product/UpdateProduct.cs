namespace Ecommerce_IntegrationTesting.Product;

[Category("v1.0")]
public sealed class UpdateProductIntegrationTest
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
    ///     Given an empty database table but a valid Product.Title payload as json string,
    ///         when request endpoint and pass all validations,
    ///             then return http-status 404 with custom problem details payload.
    /// </summary>
    [Test]
    public async Task UpdateTitle_I()
    {
        var response = await _httpClient.PutAsync("/product/8a5b3e4a-3e08-492c-869e-317a4d04616a", new StringContent("""
                {
                    "title": "Samsung TV 57"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

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
                    "instance": "/product/8a5b3e4a-3e08-492c-869e-317a4d04616a"
                }
            """)));
        });
    }

    ///  <summary>
    ///     Given products on database table with a valid Product.Title payload as json string,
    ///         when request endpoint and pass all validations,
    ///             then return http-status 202 and validate operation with extra request.
    /// </summary>
    [Test]
    public async Task UpdateTitle_II()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "title": "Samsung TV 57"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await _httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");
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

    ///  <summary>
    ///     Given an invalid Product.Title payload as json string,
    ///         when request endpoint and validate on domain service,
    ///             then return http-status 400 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task UpdateTitle_III()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "title": "j"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "BadRequest",
                    "status": 400,
                    "detail": "Product title is invalid",
                    "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
                }
            """)));
        });
    }

    ///  <summary>
    ///     Given an invalid Product.Title payload as json string,
    ///         when request endpoint and validate on endpoint,
    ///             then return http-status 400 and generic problem details payload.
    /// </summary>
    [Test]
    public async Task UpdateTitle_IV()
    {
        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "title": 89
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "Bad Request",
                    "status": 400
                }
            """)));
        });
    }


    ///  <summary>
    ///     Given products on database table with a valid Product.Description payload as json string,
    ///         when request endpoint and pass all validations,
    ///             then return http-status 202 and validate operation with extra request.
    /// </summary>
    [Test]
    public async Task UpdateDescription_I()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "description": "Great guitar, sure"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await _httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");
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

    ///  <summary>
    ///     Given an invalid Product.Description payload as json string,
    ///         when request endpoint and validate on domain service,
    ///             then return http-status 400 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task UpdateDescription_II()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "description": "o"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "BadRequest",
                    "status": 400,
                    "detail": "Product description is invalid",
                    "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
                }
            """)));
        });
    }

    ///  <summary>
    ///     Given an invalid Product.Description payload as json string,
    ///         when request endpoint and validate on endpoint,
    ///             then return http-status 400 and generic problem details payload.
    /// </summary>
    [Test]
    public async Task UpdateDescription_III()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "description": true
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "Bad Request",
                    "status": 400
                }
            """)));
        });
    }

    ///  <summary>
    ///     Given products on database table with a valid Product.Price payload as json string,
    ///         when request endpoint and pass all validations,
    ///             then return http-status 202 and validate operation with extra request.
    /// </summary>
    [Test]
    public async Task UpdatePrice_I()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "price": 500
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await _httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");
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

    ///  <summary>
    ///     Given products on database table with a invalid Product.Price payload as json string,
    ///         when request endpoint and validate on domain service,
    ///             then return http-status 400 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task UpdatePrice_II()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "price": 2
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "BadRequest",
                    "status": 400,
                    "detail": "Product price is out of range",
                    "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
                }
            """)));
        });
    }

    ///  <summary>
    ///     Given products on database table with a invalid Product.Price payload as json string,
    ///         when request endpoint and validate on endpoint,
    ///             then return http-status 400 and generic problem details payload.
    /// </summary>
    [Test]
    public async Task UpdatePrice_III()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "price": "stuff"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "Bad Request",
                    "status": 400
                }
            """)));
        });
    }

    ///  <summary>
    ///     Given products on database table with a valid Product.Status payload as json string,
    ///         when request endpoint and pass all validations,
    ///             then return http-status 202 and validate operation with extra request.
    /// </summary>
    [Test]
    public async Task UpdateStatus_I()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "status": "draft"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await _httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");
        var responseAfterUpdateBody = await responseAfterUpdate.Content.ReadAsStringAsync();

        Assert.That(responseAfterUpdateBody, Is.EqualTo(Json.MinifyString("""
            {
                "id": "092cc0ea-a54f-48a3-87ed-0e7f43c023f1",
                "title": "American Professional II Stratocaster",
                "description": "Great guitar",
                "price": 219900,
                "status": "draft"
            }
        """)));
    }

    ///  <summary>
    ///     Given products on database table with a invalid Product.Status payload as json string,
    ///         when request endpoint and validate on domain service,
    ///             then return http-status 400 and custom problem details payload.
    /// </summary>
    [Test]
    public async Task UpdateStatus_II()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "status": "yug87gu"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "BadRequest",
                    "status": 400,
                    "detail": "Product status is invalid",
                    "instance": "/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1"
                }
            """)));
        });
    }

    ///  <summary>
    ///     Given products on database table with a invalid Product.Status payload as json string,
    ///         when request endpoint and validate on endpoint,
    ///             then return http-status 400 and generic problem details payload.
    /// </summary>
    [Test]
    public async Task UpdateStatus_III()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "status": 45
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            Assert.That(responseBody, Is.EqualTo(Json.MinifyString("""
                {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "Bad Request",
                    "status": 400
                }
            """)));
        });
    }

    ///  <summary>
    ///     Given products on database table with a valid Product:Partial payload as json string,
    ///         when request endpoint and pass all validations,
    ///             then return http-status 202 and validate operation with extra request.
    /// </summary>
    [Test]
    public async Task UpdateMultiple_I()
    {
        await _server.ExecuteSqlAsync("""
            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 'published');
        """);

        var response = await _httpClient.PutAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1", new StringContent("""
                {
                    "title": "American Professional IV Stratocaster",
                    "price": 269900,
                    "status": "draft"
                }
            """, new MediaTypeHeaderValue("application/json", "utf-8")));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        var responseAfterUpdate = await _httpClient.GetAsync("/product/092cc0ea-a54f-48a3-87ed-0e7f43c023f1");
        var responseAfterUpdateBody = await responseAfterUpdate.Content.ReadAsStringAsync();

        Assert.That(responseAfterUpdateBody, Is.EqualTo(Json.MinifyString("""
            {
                "id": "092cc0ea-a54f-48a3-87ed-0e7f43c023f1",
                "title": "American Professional IV Stratocaster",
                "description": "Great guitar",
                "price": 269900,
                "status": "draft"
            }
        """)));
    }
}