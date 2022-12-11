namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Moq;
using System.Net;

using Common.Application.HttpUtil;
using Common.Fixture.Application.Tests;

using Ecommerce.Application.Command;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.DataTransfer;

[Category(TestCategory.Unit)]
public sealed class ProductUpdateById
{
    private readonly ISender _sender = Mock.Of<ISender>();

    [SetUp]
    public void BeforeEach()
    {
        Mock.Get(_sender).Reset();
    }

    [Test]
    public async Task GivenProductIdAndProduct_WhenRequestSender_ThenPass()
    {
        var productId = Common.Domain.Schema.NewID();
        var product = Mock.Of<UpdateProductHttpRequestBody>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(
                        new HttpResultResponse(CancellationToken.None)
                        {
                            StatusCode = HttpStatusCode.Accepted
                        }
                    );

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(productId, product, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());
    }
}

