namespace Ecommerce.IntegrationTest.Infrastructure.Controller;

using Dapper;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Npgsql;
using System.Net;

using Common.Application;
using Common.Fixture.Infrastructure.Database;

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

        _services.AddEcommerceServices();
        _services.AddSingleton<IDbContext>(_dbContext);
    }

    [Test]
    public async Task Given_When_Then()
    {
        await using var conn = new NpgsqlConnection(_dbContext.GetConnectionString());
        await conn.OpenAsync();
        string sql = "TRUNCATE product";
        await conn.ExecuteAsync(sql);

        var serviceProvider = _services.BuildServiceProvider();
        var serviceSender = serviceProvider.GetService<ISender>() ?? throw new ArgumentNullException();

        var controller = new ProductController(serviceSender);

        var actionResult = await controller.GetProducts(CancellationToken.None);
        Assert.That(actionResult, Is.InstanceOf<HttpResultResponse>());

        var actionContext = IntegrationTestHelpers.CreateWritableActionContext();
        await actionResult.ExecuteResultAsync(actionContext);

        Assert.That(actionContext.HttpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(IntegrationTestHelpers.ReadBodyFromActionContext(actionContext, resetPointer: true), Is.EqualTo("[]"));
    }
}
