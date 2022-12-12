namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Moq;
using System.Net;

using Common.Application.HttpUtil;

using Ecommerce.Application.Command;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.DataTransfer;

public sealed class ProductUpdateByIdUnitTest
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

        var requestBody = new UpdateProductHttpRequestBody
        {
            Title = "Super title 2",
        };

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(
                        new HttpResultResponse()
                        {
                            StatusCode = HttpStatusCode.Accepted
                        }
                    );

        var controller = new ProductController(_sender);

        var actionResult = await controller.UpdateProduct(productId, requestBody, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());
    }
}

