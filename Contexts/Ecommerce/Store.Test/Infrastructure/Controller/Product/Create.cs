namespace Ecommerce.Store.Test.Infrastructure.Controller.Product;

using Ecommerce.Store.Application.Command;
using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Exceptions;
using Ecommerce.Store.Domain.LogEvent;
using Ecommerce.Store.Infrastructure.Controller;
using Ecommerce.Store.Infrastructure.DTO;
using Common.Domain.Service;

public class Create
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
    public async Task GivenRequestCommand_WhenReturnsProductIdFromMediator_ThenReplyOK()
    {
        var newProduct = Mock.Of<NewProduct>();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<CreateProductCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Product.NewID());

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        var actionResultObject = (OkObjectResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<Guid>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductIdInvalidExceptionFromMediator_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<NewProduct>();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<CreateProductCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<ProductIdInvalidException>();

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductTitleInvalidExceptionFromMediator_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<NewProduct>();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<CreateProductCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<ProductTitleInvalidException>();

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());

        Mock
            .Get(_logger)
            .Verify(logger => logger.Print(It.Is<int>(ev => ev == ProductLogEvent.CreateBadRequest), It.IsAny<string>()));
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductDescriptionInvalidExceptionFromMediator_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<NewProduct>();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<CreateProductCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<ProductDescriptionInvalidException>();

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());

        Mock
            .Get(_logger)
            .Verify(logger => logger.Print(It.Is<int>(ev => ev == ProductLogEvent.CreateBadRequest), It.IsAny<string>()));
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductPriceInvalidExceptionFromMediator_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<NewProduct>();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<CreateProductCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<ProductPriceInvalidException>();

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());

        Mock
            .Get(_logger)
            .Verify(logger => logger.Print(It.Is<int>(ev => ev == ProductLogEvent.CreateBadRequest), It.IsAny<string>()));
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsProductStatusInvalidExceptionFromMediator_ThenReplyBadRequest()
    {
        var newProduct = Mock.Of<NewProduct>();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<CreateProductCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<ProductStatusInvalidException>();

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<BadRequestResult>());

        Mock
            .Get(_logger)
            .Verify(logger => logger.Print(It.Is<int>(ev => ev == ProductLogEvent.CreateBadRequest), It.IsAny<string>()));
    }

    [Test]
    public async Task GivenRequestCommand_WhenThrowsAnyExceptionFromMediator_ThenReplyNotImplemented()
    {
        var newProduct = Mock.Of<NewProduct>();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<CreateProductCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<Exception>();

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.Create(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));

        Mock
            .Get(_logger)
            .Verify(logger => logger.Print(It.Is<int>(ev => ev == ProductLogEvent.CreateNotImplemented), It.IsAny<string>()));
    }
}
