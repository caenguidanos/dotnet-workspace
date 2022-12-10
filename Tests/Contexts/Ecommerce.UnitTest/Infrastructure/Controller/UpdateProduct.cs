namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;

using Common.Application.HttpUtil;
using Ecommerce.Application.Command;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.DataTransfer;

public class ProductUpdateById
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
                            StatusCode = StatusCodes.Status202Accepted
                        }
                    );

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(productId, product, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());
    }
}

