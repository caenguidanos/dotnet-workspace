namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

using Ecommerce.Application.Query;
using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Model;
using Ecommerce.Domain.ValueObject;
using Ecommerce.Infrastructure.Controller;

public class ProductGetById
{
    private readonly ISender _sender = Mock.Of<ISender>();

    [SetUp]
    public void BeforeEach()
    {
        Mock.Get(_sender).Reset();
    }

    [Test]
    public async Task GivenRequestQuery_WhenReturnsProductFromSender_ThenReplyWithProduct()
    {
        var product = new Product(
            new ProductId(Common.Domain.Schema.NewID()),
            new ProductTitle("Title 1"),
            new ProductDescription("Description 1"),
            new ProductStatus(ProductStatusValue.Closed),
            new ProductPrice(100));

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<GetProductQuery>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(product);

        var controller = new ProductController(_sender);

        var actionResult = await controller.GetProductById(product.Id, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        var actionResultObject = (OkObjectResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<Product>());
    }

    [Test]
    public async Task GivenRequestQuery_WhenThrowsAnyExceptionFromSender_ThenReplyWithNotImplemented()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<GetProductQuery>(),
                    It.IsAny<CancellationToken>())).Throws<Exception>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.GetProductById(Common.Domain.Schema.NewID(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));
    }

    [Test]
    public async Task GivenRequestQuery_WhenThrowsProductNotFoundException_ThenReplyWithNotFound()
    {
        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<GetProductQuery>(),
                    It.IsAny<CancellationToken>())).Throws<ProductNotFoundException>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.GetProductById(Common.Domain.Schema.NewID(), CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
    }
}
