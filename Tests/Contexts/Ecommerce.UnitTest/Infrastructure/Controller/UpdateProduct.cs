namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

using Ecommerce.Application.Command;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.DataTransfer;

public class ProductUpdateById
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
                    It.IsAny<UpdateProductCommand>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(Common.Domain.Schema.NewID(), Mock.Of<ProductPrimitivesForUpdateOperation>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<AcceptedResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductNotFoundExceptionFromSender_ThenReplyWithNotFound()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<UpdateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductNotFoundException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(Common.Domain.Schema.NewID(), Mock.Of<ProductPrimitivesForUpdateOperation>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductTitleInvalidExceptionFromSender_ThenReplyWithBadRequest()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<UpdateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductTitleInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(Common.Domain.Schema.NewID(), Mock.Of<ProductPrimitivesForUpdateOperation>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductDescriptionInvalidExceptionFromSender_ThenReplyWithBadRequest()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<UpdateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductDescriptionInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(Common.Domain.Schema.NewID(), Mock.Of<ProductPrimitivesForUpdateOperation>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductPriceInvalidExceptionFromSender_ThenReplyWithBadRequest()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<UpdateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductPriceInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(Common.Domain.Schema.NewID(), Mock.Of<ProductPrimitivesForUpdateOperation>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductStatusInvalidExceptionFromSender_ThenReplyWithBadRequest()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<UpdateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductStatusInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(Common.Domain.Schema.NewID(), Mock.Of<ProductPrimitivesForUpdateOperation>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsAnyExceptionFromSender_ThenReplyWithNotImplemented()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<UpdateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<Exception>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(Common.Domain.Schema.NewID(), Mock.Of<ProductPrimitivesForUpdateOperation>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));
    }
}

