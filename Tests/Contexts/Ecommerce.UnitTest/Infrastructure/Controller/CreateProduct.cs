namespace Ecommerce.UnitTest.Infrastructure.Controller;

using MediatR;
using Moq;
using System.Net;

using Common.Application.HttpUtil;

using Ecommerce.Application.Command;
using Ecommerce.Infrastructure.Controller;
using Ecommerce.Infrastructure.DataTransfer;
using Ecommerce.Domain.Model;

public sealed class CreateProductUnitTest
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
        var requestBody = new CreateProductHttpRequestBody
        {
            Title = "Super title 1",
            Description = "Super description 1",
            Status = (int)ProductStatusValue.Published,
            Price = 200
        };

        Mock
            .Get(_sender)
            .Setup(sender => sender
                .Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(
                        new HttpResultResponse()
                        {
                            StatusCode = HttpStatusCode.OK
                        }
                    );

        var controller = new ProductController(_sender);

        var actionResult = await controller.CreateProduct(requestBody, CancellationToken.None);

        Assert.That(actionResult, Is.TypeOf<HttpResultResponse>());
    }
}
