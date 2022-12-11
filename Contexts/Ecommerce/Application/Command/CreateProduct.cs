namespace Ecommerce.Application.Command;

using MediatR;
using System.Net;
using System.Net.Mime;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct CreateProductCommand : IRequest<HttpResultResponse>
{
    public required int Price { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Status { get; init; }
}

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, HttpResultResponse>
{
    private readonly IProductService productService;

    public CreateProductHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<HttpResultResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var createdProductId = await productService.AddNewProduct(
                  request.Title,
                  request.Description,
                  request.Status,
                  request.Price,
                  cancellationToken);

            return new HttpResultResponse(cancellationToken)
            {
                Body = new ProductAck { Id = createdProductId },
                StatusCode = HttpStatusCode.OK,
                ContentType = MediaTypeNames.Application.Json,
            };
        }
        catch (Exception ex)
        {
            if (ex
                is ProductTitleInvalidException
                or ProductDescriptionInvalidException
                or ProductPriceInvalidException
                or ProductStatusInvalidException
                or ProductTitleUniqueException)
            {
                return new HttpResultResponse(cancellationToken)
                {
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }

            if (ex is ProductPersistenceException)
            {
                return new HttpResultResponse(cancellationToken)
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                };
            }

            return new HttpResultResponse(cancellationToken)
            {
                StatusCode = HttpStatusCode.NotImplemented,
            };
        }
    }
}
