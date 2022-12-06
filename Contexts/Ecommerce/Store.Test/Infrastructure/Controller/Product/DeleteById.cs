namespace Ecommerce.Store.Test.Infrastructure.Controller.Product;

using Ecommerce.Store.Application.Command;
using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Exceptions;
using Ecommerce.Store.Infrastructure.Controller;
using Common.Domain.Service;
using Ecommerce.Store.Domain.LogEvent;

public class DeleteById
{
    private readonly IMediator _mediator = Mock.Of<IMediator>();
    private readonly ILoggerService _logger = Mock.Of<ILoggerService>();

    [SetUp]
    public void BeforeEach()
    {
        Mock.Get(_logger).Reset();
        Mock.Get(_mediator).Reset();
    }

    [Test]
    public async Task GivenRequestCommand_WhenReturnsIdFromMediator_ThenReplyWithOK()
    {
        var id = Product.NewID();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<DeleteProductCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(id);

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.DeleteById(id, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        var actionResultObject = (OkObjectResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<Guid>());

        var actionResultObjectPayload = (Guid)actionResultObject.Value;
        Assert.That(actionResultObjectPayload, Is.EqualTo(id));
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductNotFoundExceptionFromMediator_ThenReplyWithNotFound()
    {
        var id = Product.NewID();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<DeleteProductCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<ProductNotFoundException>();

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.DeleteById(id, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<NotFoundResult>());

        Mock
            .Get(_logger)
            .Verify(logger => logger.Print(It.Is<int>(ev => ev == ProductLogEvent.DeleteByIdNotFound), It.IsAny<string>()));
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsAnyExceptionFromMediator_ThenReplyWithNotImplemented()
    {
        var id = Product.NewID();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<DeleteProductCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<Exception>();

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.DeleteById(id, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));

        Mock
            .Get(_logger)
            .Verify(logger => logger.Print(It.Is<int>(ev => ev == ProductLogEvent.DeleteByIdNotImplemented), It.IsAny<string>()));
    }
}

