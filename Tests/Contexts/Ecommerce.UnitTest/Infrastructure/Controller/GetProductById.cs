namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;

using Common.Application.HttpUtil;
using Ecommerce.Application.Query;
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
    public async Task GivenProductId_WhenRequestSender_ThenPass()
    {
        var productId = Common.Domain.Schema.NewID();

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(
                        new HttpResultResponse(CancellationToken.None)
                        {
                            StatusCode = StatusCodes.Status200OK
                        }
                    );

        var controller = new ProductController(_sender);

        var actionResult = await controller.GetProductById(productId, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());
    }
}
