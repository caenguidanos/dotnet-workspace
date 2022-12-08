namespace Ecommerce.Store.Test.Infrastructure.Controller;

using Ecommerce.Store.Application.Command;
using Ecommerce.Store.Domain.Exceptions;
using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Infrastructure.Controller;
using Ecommerce.Store.Infrastructure.DataTransfer;

public class Create
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
        var newProduct = Mock.Of<ProductPrimitivesForCreateOperation>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new ProductAck { Id = Product.NewID() });

        var controller = new ProductController(_sender);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<AcceptedResult>());

        var actionResultObject = (AcceptedResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<ProductAck>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductTitleInvalidExceptionFromSender_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<ProductPrimitivesForCreateOperation>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductTitleInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductDescriptionInvalidExceptionFromSender_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<ProductPrimitivesForCreateOperation>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductDescriptionInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductPriceInvalidExceptionFromSender_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<ProductPrimitivesForCreateOperation>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductPriceInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductStatusInvalidExceptionFromSender_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<ProductPrimitivesForCreateOperation>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductStatusInvalidException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsAnyExceptionFromSender_ThenReplyNotImplemented()
    {
        var newProduct = Mock.Of<ProductPrimitivesForCreateOperation>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<CreateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<Exception>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));
    }
}
