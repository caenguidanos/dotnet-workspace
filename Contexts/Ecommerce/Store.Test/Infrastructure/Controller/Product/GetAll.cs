namespace Ecommerce.Store.Test.Infrastructure.Controller.Product;

using Ecommerce.Store.Application.Query;
using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.ValueObject;
using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Infrastructure.Controller;
using Common.Domain.Service;
using Ecommerce.Store.Domain.LogEvent;

public class GetAll
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
    public async Task GivenRequestQuery_WhenReturnsProductsFromMediator_ThenReplyWithProducts()
    {
        var products = new List<Product>
        {
            new Product(
                    new ProductId(Product.NewID()),
                    new ProductTitle("Title 1"),
                    new ProductDescription("Description 1"),
                    new ProductStatus(ProductStatusValue.Draft),
                    new ProductPrice(100)),

            new Product(
                    new ProductId(Product.NewID()),
                    new ProductTitle("Title 2"),
                    new ProductDescription("Description 2"),
                    new ProductStatus(ProductStatusValue.Published),
                    new ProductPrice(200))
        };

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<GetProductsQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.GetAll(CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        var actionResultObject = (OkObjectResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<List<Product>>());

        var actionResultObjectPayload = (List<Product>)actionResultObject.Value;
        Assert.That(actionResultObjectPayload.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GivenRequestQuery_WhenReturnsNoProductsFromMediator_ThenReplyWithEmptyList()
    {
        var products = new List<Product>();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<GetProductsQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.GetAll(CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        var actionResultObject = (OkObjectResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<List<Product>>());

        var actionResultObjectPayload = (List<Product>)actionResultObject.Value;
        Assert.That(actionResultObjectPayload, Is.Empty);
    }

    [Test]
    public async Task GivenRequestQuery_WhenThrowsAnyExceptionFromMediator_ThenReplyWithNotImplemented()
    {
        var products = Mock.Of<List<Product>>();

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<GetProductsQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<Exception>();

        var controller = new ProductController(_logger, _mediator);

        var actionResult = await controller.GetAll(CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));

        Mock
            .Get(_logger)
            .Verify(logger => logger.Print(It.Is<int>(ev => ev == ProductLogEvent.GetAllNotImplemented), It.IsAny<string>()));
    }
}
