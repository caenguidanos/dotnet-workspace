namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;

using Common.Application.HttpUtil;
using Ecommerce.Application.Command;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.DataTransfer;

public class CreateProduct
{
    private readonly ISender _sender = Mock.Of<ISender>();

    [SetUp]
    public void BeforeEach()
    {
        Mock.Get(_sender).Reset();
    }

    [Test]
    public async Task GivenNewProduct_WhenRequestSender_ThenPass()
    {
        var newProduct = Mock.Of<CreateProductHttpRequestBody>();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(
                        new HttpResultResponse(CancellationToken.None)
                        {
                            StatusCode = StatusCodes.Status200OK
                        }
                    );

        var controller = new ProductController(_sender);

        var actionResult = await controller.CreateProduct(newProduct, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());
    }
}
