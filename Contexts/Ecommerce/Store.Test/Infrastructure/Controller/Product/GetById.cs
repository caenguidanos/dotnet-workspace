namespace Ecommerce.Store.Test.Infrastructure.Controller.Product;

using Ecommerce.Store.Application.Query;
using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Exceptions;
using Ecommerce.Store.Domain.Model;
using Ecommerce.Store.Domain.Notification;
using Ecommerce.Store.Domain.ValueObject;
using Ecommerce.Store.Infrastructure.Controller;

public class GetById
{
    private readonly IMediator _mediator = Mock.Of<IMediator>();

    [SetUp]
    public void BeforeEach()
    {
        Mock.Get(_mediator).Reset();
    }

    [Test]
    public async Task GivenRequestQuery_WhenReturnsProductFromMediator_ThenReplyWithProduct()
    {
        var product = new Product(
            new ProductId(Product.NewID()),
            new ProductTitle("Title 1"),
            new ProductDescription("Description 1"),
            new ProductStatus(ProductStatusValue.Draft),
            new ProductPrice(100));

        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<GetProductQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var controller = new ProductController(_mediator);

        var actionResult = await controller.GetById(product.Id, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        var actionResultObject = (OkObjectResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<Product>());
    }

    [Test]
    public async Task GivenRequestQuery_WhenThrowsAnyExceptionFromMediator_ThenReplyWithNotImplemented()
    {
        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<GetProductQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<Exception>();

        var controller = new ProductController(_mediator);

        var actionResult = await controller.GetById(Product.NewID(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));

        Mock
            .Get(_mediator)
            .Verify(mediator => mediator.Publish(
                It.Is<ProductLogNotification>(
                    notification => notification.Event == ProductLog.GetByIdNotImplemented),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task GivenRequestQuery_WhenThrowsProductNotFoundException_ThenReplyWithNotFound()
    {
        Mock
            .Get(_mediator)
            .Setup(self => self.Send(
                It.IsAny<GetProductQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<ProductNotFoundException>();

        var controller = new ProductController(_mediator);

        var actionResult = await controller.GetById(Product.NewID(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<NotFoundResult>());

        Mock
            .Get(_mediator)
            .Verify(mediator => mediator.Publish(
                It.Is<ProductLogNotification>(
                    notification => notification.Event == ProductLog.GetByIdNotFound),
                It.IsAny<CancellationToken>()));
    }
}
