namespace Ecommerce.Store.Test.Infrastructure.Controller.Product;

using Ecommerce.Store.Application.Command;
using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Exceptions;
using Ecommerce.Store.Infrastructure.Controller;
using Ecommerce.Store.Infrastructure.DataTransfer;

public class UpdateById
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

        var actionResult = await controller.UpdateById(Guid.NewGuid(), Mock.Of<PartialProduct>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<AcceptedResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductNotFoundExceptionFromSender_ThenReplyWithNotFound()
    {
        var id = Product.NewID();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<UpdateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductNotFoundException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateById(Guid.NewGuid(), Mock.Of<PartialProduct>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsAnyExceptionFromSender_ThenReplyWithNotImplemented()
    {
        var id = Product.NewID();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<UpdateProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<Exception>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateById(Guid.NewGuid(), Mock.Of<PartialProduct>(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));
    }
}

