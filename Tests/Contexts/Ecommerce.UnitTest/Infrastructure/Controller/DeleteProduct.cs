namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

using Ecommerce.Application.Command;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Infrastructure.Controller;

public class ProductDeleteById
{
    private readonly ISender _sender = Mock.Of<ISender>();

    [SetUp]
    public void BeforeEach()
    {
        Mock.Get(_sender).Reset();
    }

    [Test]
    public async Task GivenRequestCommand_WhenReturnsNothingFromSender_ThenReplyWithAccepted()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<DeleteProductCommand>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

        var controller = new ProductController(_sender);

        var actionResult = await controller.DeleteProduct(Common.Domain.Schema.NewID(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<AcceptedResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductNotFoundExceptionFromSender_ThenReplyWithNotFound()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<DeleteProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductNotFoundException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.DeleteProduct(Common.Domain.Schema.NewID(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsPersistenceExceptionFromSender_ThenReplyWithServiceUnavailable()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<DeleteProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductRepositoryPersistenceException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.DeleteProduct(Common.Domain.Schema.NewID(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status503ServiceUnavailable));
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsAnyExceptionFromSender_ThenReplyWithNotImplemented()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<DeleteProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<Exception>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.DeleteProduct(Common.Domain.Schema.NewID(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));
    }
}

