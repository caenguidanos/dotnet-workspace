namespace Ecommerce.IntegrationTest.Infrastructure.Controller;

using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;

using Common.Extensions;
using Common.Application;
using Common.Fixture.Infrastructure.Database;

using Ecommerce.Extensions;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.Persistence;

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

    [Test]
    public async Task GivenNoProductsOnDatabase_WhenQueryAll_ThenReturnEmptyCollection()
    {
        await IntegrationTestHelpers.ExecuteQueryAsync(_dbContext.GetConnectionString(), "TRUNCATE product");

        var provider = _services.BuildServiceProvider();

        var controller = new ProductController(
            provider.GetService<ISender>() ?? throw new ArgumentNullException()
        );

        var actionContext = IntegrationTestHelpers.CreateWritableActionContext();

        var actionResult = await controller.GetProducts(CancellationToken.None);
        Assert.That(actionResult, Is.InstanceOf<HttpResultResponse>());

        await actionResult.ExecuteResultAsync(actionContext);

        Assert.That(actionContext.HttpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(
            IntegrationTestHelpers.ReadBodyFromActionContext(actionContext, resetPointer: true), Is.EqualTo("[]"));
    }
}
