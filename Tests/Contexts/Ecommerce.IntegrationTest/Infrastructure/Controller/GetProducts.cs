namespace Ecommerce.IntegrationTest.Infrastructure.Controller;

using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;

using Common.Extensions;
using Common.Application;
using Common.Fixture.Infrastructure.Database;

using Ecommerce.Extensions;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.DataTransfer;
using Ecommerce.Domain.Model;

public sealed class GetProductsIntegrationTest
{
    private readonly PostgresDatabaseFactory _postgres = new(template: "ecommerce");
    private readonly IDbContext _dbContext = Mock.Of<IDbContext>();
    private readonly IServiceCollection _services = new ServiceCollection();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        string connectionString = await _postgres.StartAsync();

        Mock
            .Get(_dbContext)
            .Setup(dbContext => dbContext.GetConnectionString())
                .Returns(connectionString);

        _services.AddCommonConfig();
        _services.AddEcommerceServices();
        _services.AddSingleton<IDbContext>(_dbContext);
    }

    [Test, Order(1)]
    public async Task GivenNoProductsOnDatabase_WhenQueryAll_ThenReturnEmptyCollection()
    {
        await IntegrationTestHelpers.ExecuteQueryAsync(_dbContext.GetConnectionString(), "TRUNCATE product");

        var provider = _services.BuildServiceProvider();

        var controller = new ProductController(provider.GetService<ISender>() ?? throw new ArgumentNullException());

        var actionResult = await controller.GetProducts(CancellationToken.None);
        Assert.That(actionResult, Is.InstanceOf<HttpResultResponse>());

        var actionContext = IntegrationTestHelpers.CreateWritableActionContext();
        await actionResult.ExecuteResultAsync(actionContext);

        Assert.That(actionContext.HttpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(
            IntegrationTestHelpers.ReadBodyFromActionContext(actionContext, resetPointer: true), Is.EqualTo("[]"));
    }

    [Test, Order(2)]
    public async Task GivenProductsOnDatabase_WhenQueryAll_ThenReturnCollection()
    {
        await IntegrationTestHelpers.ExecuteQueryAsync(_dbContext.GetConnectionString(),
        @"
            TRUNCATE product;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 219900, 1);

            INSERT INTO product (id, title, description, price, status)
            VALUES ('8a5b3e4a-3e08-492c-869e-317a4d04616a', 'Mustang Shelby GT500', 'Great car', 7900000, 1);
        ");

        var provider = _services.BuildServiceProvider();

        var controller = new ProductController(provider.GetService<ISender>() ?? throw new ArgumentNullException());

        var actionResult = await controller.GetProducts(CancellationToken.None);
        Assert.That(actionResult, Is.InstanceOf<HttpResultResponse>());

        var actionContext = IntegrationTestHelpers.CreateWritableActionContext();
        await actionResult.ExecuteResultAsync(actionContext);

        Assert.That(actionContext.HttpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        var products = IntegrationTestHelpers.JsonDeserialize<ProductPrimitives[]>(
            IntegrationTestHelpers.ReadBodyFromActionContext(actionContext, resetPointer: true)
        );

        Assert.That(products, Is.Not.Null);
        if (products is not null)
        {
            Assert.That(products.Count(), Is.EqualTo(2));

            var productA = products.First(product => product.Title == "American Professional II Stratocaster");
            var productB = products.First(product => product.Title == "Mustang Shelby GT500");

            Assert.That(productA, Is.Not.Null);
            Assert.That(productB, Is.Not.Null);

            if (productA is not null)
            {
                Assert.That(productA.Id, Is.EqualTo(Guid.Parse("092cc0ea-a54f-48a3-87ed-0e7f43c023f1")));
                Assert.That(productA.Description, Is.EqualTo("Great guitar"));
                Assert.That(productA.Price, Is.EqualTo(219900));
                Assert.That(productA.Status, Is.EqualTo(ProductStatusValue.Published));
                Assert.That(productA.created_at, Is.Not.EqualTo(default));
                Assert.That(productA.updated_at, Is.Not.EqualTo(default));
            }

            if (productB is not null)
            {
                Assert.That(productB.Id, Is.EqualTo(Guid.Parse("8a5b3e4a-3e08-492c-869e-317a4d04616a")));
                Assert.That(productB.Description, Is.EqualTo("Great car"));
                Assert.That(productB.Price, Is.EqualTo(7900000));
                Assert.That(productB.Status, Is.EqualTo(ProductStatusValue.Published));
                Assert.That(productB.created_at, Is.Not.EqualTo(default));
                Assert.That(productB.updated_at, Is.Not.EqualTo(default));
            }
        }
    }

    [Test]
    public async Task GivenIlegalProductOnDatabase_WhenQueryAll_ThenReturnProblemDetails()
    {
        await IntegrationTestHelpers.ExecuteQueryAsync(_dbContext.GetConnectionString(),
        @"
            TRUNCATE product;

            ALTER TABLE product DROP CONSTRAINT check_price_range;

            INSERT INTO product (id, title, description, price, status)
            VALUES ('092cc0ea-a54f-48a3-87ed-0e7f43c023f1', 'American Professional II Stratocaster', 'Great guitar', 0, 1);
        ");

        var provider = _services.BuildServiceProvider();

        // add custom httpContext for Request.Path reference on GetProblemDetails(Request.Path)
        var controllerHttpContext = new DefaultHttpContext();
        controllerHttpContext.Request.Path = "/anything";

        var controller = new ProductController(provider.GetService<ISender>() ?? throw new ArgumentNullException())
        {
            ControllerContext = new ControllerContext() { HttpContext = controllerHttpContext }
        };

        var actionResult = await controller.GetProducts(CancellationToken.None);
        Assert.That(actionResult, Is.InstanceOf<HttpResultResponse>());

        var actionContext = IntegrationTestHelpers.CreateWritableActionContext();
        await actionResult.ExecuteResultAsync(actionContext);

        Assert.That(actionContext.HttpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));

        var problemDetails = IntegrationTestHelpers.JsonDeserialize<ProblemDetails>(
            IntegrationTestHelpers.ReadBodyFromActionContext(actionContext, resetPointer: true)
        );

        Assert.That(problemDetails, Is.Not.Null);
        if (problemDetails is not null)
        {
            Assert.That(problemDetails.Title, Is.EqualTo("BadRequest"));
            Assert.That(problemDetails.Instance, Is.EqualTo("/anything"));
            Assert.That(problemDetails.Detail, Is.EqualTo("Product price is out of range"));
            Assert.That(problemDetails.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }
    }

}
