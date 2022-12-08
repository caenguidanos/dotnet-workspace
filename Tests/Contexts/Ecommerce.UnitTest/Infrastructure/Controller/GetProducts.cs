namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

using global::Ecommerce.Application.Query;
using global::Ecommerce.Domain.Entity;
using global::Ecommerce.Domain.Model;
using global::Ecommerce.Domain.ValueObject;
using global::Ecommerce.Infrastructure.Controller;

public class ProductGetAll
{
    private readonly ISender _sender = Mock.Of<ISender>();

    [SetUp]
    public void BeforeEach()
    {
        Mock.Get(_sender).Reset();
    }

    [Test]
    public async Task GivenRequestQuery_WhenReturnsProductsFromSender_ThenReplyWithProducts()
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
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<GetProductsQuery>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(products);

        var controller = new ProductController(_sender);

        var actionResult = await controller.Get(CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        var actionResultObject = (OkObjectResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<List<Product>>());

        var actionResultObjectPayload = (List<Product>)actionResultObject.Value;
        Assert.That(actionResultObjectPayload.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GivenRequestQuery_WhenReturnsNoProductsFromSender_ThenReplyWithEmptyList()
    {
        var products = new List<Product>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<GetProductsQuery>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(products);

        var controller = new ProductController(_sender);

        var actionResult = await controller.Get(CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        var actionResultObject = (OkObjectResult)actionResult;
        Assert.That(actionResultObject.Value, Is.TypeOf<List<Product>>());

        var actionResultObjectPayload = (List<Product>)actionResultObject.Value;
        Assert.That(actionResultObjectPayload, Is.Empty);
    }

    [Test]
    public async Task GivenRequestQuery_WhenThrowsAnyExceptionFromSender_ThenReplyWithNotImplemented()
    {
        var products = Mock.Of<List<Product>>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(
                    It.IsAny<GetProductsQuery>(),
                    It.IsAny<CancellationToken>())).Throws<Exception>();

        var controller = new ProductController(_sender);

        var actionResult = await controller.Get(CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<StatusCodeResult>());

        var actionResultObject = (StatusCodeResult)actionResult;
        Assert.That(actionResultObject.StatusCode, Is.EqualTo(StatusCodes.Status501NotImplemented));
    }
}
