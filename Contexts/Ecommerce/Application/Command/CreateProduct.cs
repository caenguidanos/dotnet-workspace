namespace Ecommerce.Application.Command;

using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger _logger;
    private readonly IProductCreatorService _productCreatorService;

    public CreateProductHandler(ILogger<CreateProductHandler> logger, IProductCreatorService productCreatorService)
    {
        _logger = logger;
        _productCreatorService = productCreatorService;
    }

    public async Task<HttpResultResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var createdProductId = await _productCreatorService.AddNewProduct(
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
            _logger.LogError(ex.ToString());

            if (ex
                is ProductTitleInvalidException
                or ProductTitleUniqueException
                or ProductDescriptionInvalidException
                or ProductPriceInvalidException
                or ProductStatusInvalidException)
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
