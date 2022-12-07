namespace Ecommerce.Store.Test.Infrastructure.Controller.Product;

using Ecommerce.Store.Application.Command;
using Ecommerce.Store.Domain.Exceptions;
using Ecommerce.Store.Infrastructure.Controller;

public class DeleteById
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
        var id = Common.Domain.Entity.Entity.NewID();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<DeleteProductCommand>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

        var controller = new ProductController(_sender);

        var actionResult = await controller.DeleteById(id, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<AcceptedResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductNotFoundExceptionFromSender_ThenReplyWithNotFound()
    {
        var id = Common.Domain.Entity.Entity.NewID();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<DeleteProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<ProductNotFoundException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.DeleteById(id, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsAnyExceptionFromSender_ThenReplyWithNotImplemented()
    {
        var id = Common.Domain.Entity.Entity.NewID();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<DeleteProductCommand>(),
                    It.IsAny<CancellationToken>())).Throws<Exception>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.DeleteById(id, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));
    }
}

