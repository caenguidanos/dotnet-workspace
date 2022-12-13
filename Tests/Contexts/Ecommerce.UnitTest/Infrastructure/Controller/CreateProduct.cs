namespace Ecommerce.UnitTest.Infrastructure.Controller;

using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

using Common.Application;

using Ecommerce.Application.Command;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.DataTransfer;

public sealed class CreateProductUnitTest
{
    private readonly ISender _sender = Mock.Of<ISender>();
    private readonly IHttpExceptionManager _exceptionManager = new ExceptionManager();

    [SetUp]
    public void BeforeEach()
    {
        Mock.Get(_sender).Reset();
    }

    [Test]
    public async Task GivenCreateProductHttpRequestBody_WhenSenderSendsCommand_ThenReturnOK()
    {
        var payload = new ProductAck { Id = Common.Domain.Schema.NewID() };
        var payloadStringified = JsonSerializer.Serialize(payload, HttpResultResponse.serializerOptions);

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(payload);

        var httpRequestBody = new CreateProductHttpRequestBody
        {
            Title = "Super title 1",
            Description = "Super description 1",
            Status = (int)ProductStatusValue.Published,
            Price = 200
        };

        var controller = new ProductController(_sender, _exceptionManager);
        var actionResult = await controller.CreateProduct(httpRequestBody, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());

        var actionContext = new ActionContext();
        actionContext.HttpContext = new DefaultHttpContext();
        actionContext.HttpContext.Response.Body = new MemoryStream();

        await actionResult.ExecuteResultAsync(actionContext);
        Assert.That(actionContext.HttpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        Assert.That(actionContext.HttpContext.Response.ContentType, Is.EqualTo(MediaTypeNames.Application.Json));
        Assert.That(actionContext.HttpContext.Response.ContentLength, Is.EqualTo(payloadStringified.Length));

        actionContext.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var payloadAsStream = new StreamReader(actionContext.HttpContext.Response.Body);
        var payloadFromStream = await payloadAsStream.ReadToEndAsync();
        Assert.That(payloadFromStream, Is.EqualTo(payloadStringified));
    }

    [Test]
    public async Task GivenCreateProductHttpRequestBody_WhenSenderThrowsControlledException_ThenReturnProblemDetails()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ProductTitleInvalidException());

        var httpRequestBody = new CreateProductHttpRequestBody
        {
            Title = "Super title 1",
            Description = "Super description 1",
            Status = (int)ProductStatusValue.Published,
            Price = 200
        };

        var controller = new ProductController(_sender, _exceptionManager);
        var actionResult = await controller.CreateProduct(httpRequestBody, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());

        var actionContext = new ActionContext();
        actionContext.HttpContext = new DefaultHttpContext();

        await actionResult.ExecuteResultAsync(actionContext);
        Assert.That(actionContext.HttpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        Assert.That(actionContext.HttpContext.Response.ContentType, Is.EqualTo("application/problem+json"));
    }

    [Test]
    public async Task GivenCreateProductHttpRequestBody_WhenSenderThrowsUncontrolledException_ThenReturnProblemDetails()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentNullException());

        var httpRequestBody = new CreateProductHttpRequestBody
        {
            Title = "Super title 1",
            Description = "Super description 1",
            Status = (int)ProductStatusValue.Published,
            Price = 200
        };

        var controller = new ProductController(_sender, _exceptionManager);
        var actionResult = await controller.CreateProduct(httpRequestBody, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());

        var actionContext = new ActionContext();
        actionContext.HttpContext = new DefaultHttpContext();

        await actionResult.ExecuteResultAsync(actionContext);
        Assert.That(actionContext.HttpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.NotImplemented));
        Assert.That(actionContext.HttpContext.Response.ContentType, Is.EqualTo("application/problem+json"));
    }
}
