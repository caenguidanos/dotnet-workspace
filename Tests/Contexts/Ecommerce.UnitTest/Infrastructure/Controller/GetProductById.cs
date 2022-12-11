namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Moq;
using System.Net;

using Common.Application.HttpUtil;
using Common.Fixture.Application.Tests;

using Ecommerce.Application.Query;
using Ecommerce.Infrastructure.Controller;

[Category(TestCategory.Unit)]
public sealed class ProductGetById
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
                            StatusCode = HttpStatusCode.OK
                        }
                    );

        var controller = new ProductController(_sender);

        var actionResult = await controller.GetProductById(productId, CancellationToken.None);
        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());
    }
}
