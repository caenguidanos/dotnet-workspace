namespace Ecommerce.IntegrationTest.Infrastructure.Controller;

using Dapper;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        _services.AddEcommerceServices();

        string connectionString = await _postgres.StartAsync();
        Mock
            .Get(_dbContext)
            .Setup(dbContext => dbContext.GetConnectionString())
            .Returns(connectionString);

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

        var controller = new ProductController(serviceProvider.GetService<ISender>() ?? throw new ArgumentNullException());

        var actionResultFromController = await controller.GetProducts(CancellationToken.None);
        Assert.That(actionResultFromController, Is.InstanceOf<HttpResultResponse>());

        var actionContext = new ActionContext();
        actionContext.HttpContext = new DefaultHttpContext();
        actionContext.HttpContext.Response.Body = new MemoryStream();

        await actionResultFromController.ExecuteResultAsync(actionContext);
        Assert.That(actionContext.HttpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        // move pointer to start position of the stream and read body
        actionContext.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBodyReader = new StreamReader(actionContext.HttpContext.Response.Body);
        var responseBodyAsText = responseBodyReader.ReadToEnd();
        Assert.That(responseBodyAsText, Is.EqualTo("[]"));
    }
}
