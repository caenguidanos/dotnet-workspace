namespace Ecommerce.Application.Command;

using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

using Common.Application.HttpUtil;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.DataTransfer;

public class CreateProductCommand : IRequest<HttpResultResponse>
{
    public required int Price { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Status { get; init; }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, HttpResultResponse>
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
                StatusCode = StatusCodes.Status200OK,
                ContentType = MediaTypeNames.Application.Json,
            };
        }
        catch (Exception ex)
        {
            if (ex
                is ProductTitleInvalidException
                or ProductDescriptionInvalidException
                or ProductPriceInvalidException
                or ProductStatusInvalidException)
            {
                return new HttpResultResponse(cancellationToken)
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }

            if (ex is ProductPersistenceException)
            {
                return new HttpResultResponse(cancellationToken)
                {
                    StatusCode = StatusCodes.Status503ServiceUnavailable,
                };
            }

            return new HttpResultResponse(cancellationToken)
            {
                StatusCode = StatusCodes.Status501NotImplemented,
            };
        }
    }
}
