namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

using Ecommerce.Application.Command;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.DataTransfer;

public class CreateProduct
{
    private readonly ISender _sender = Mock.Of<ISender>();

    [SetUp]
    public void BeforeEach()
    {
        Mock.Get(_sender).Reset();
    }

    [Test]
    public async Task GivenRequestCommand_WhenReturnsProductAckFromSender_ThenReplyAccepted()
    {
        var newProduct = Mock.Of<CreateProductHttpRequestBody>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new ProductAck { Id = Common.Domain.Schema.NewID() });

        var controller = new ProductController(_sender);

        var actionResult = await controller.CreateProduct(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<AcceptedResult>());

        var actionResultObject = (AcceptedResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<ProductAck>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductTitleInvalidExceptionFromSender_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<CreateProductHttpRequestBody>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductTitleInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.CreateProduct(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductDescriptionInvalidExceptionFromSender_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<CreateProductHttpRequestBody>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductDescriptionInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.CreateProduct(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductPriceInvalidExceptionFromSender_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<CreateProductHttpRequestBody>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductPriceInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.CreateProduct(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductStatusInvalidExceptionFromSender_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<CreateProductHttpRequestBody>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductStatusInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.CreateProduct(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsPersistenceExceptionFromSender_ThenReplyServiceUnavailable()
    {
        var newProduct = Mock.Of<CreateProductHttpRequestBody>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductRepositoryPersistenceException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.CreateProduct(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status503ServiceUnavailable));
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsAnyExceptionFromSender_ThenReplyNotImplemented()
    {
        var newProduct = Mock.Of<CreateProductHttpRequestBody>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<Exception>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.CreateProduct(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));
    }
}
