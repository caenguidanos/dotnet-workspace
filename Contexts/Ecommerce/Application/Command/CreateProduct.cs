namespace Ecommerce.Application.Command;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mime;

using Common.Application.HttpUtil;
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
    private readonly IProductCreatorService _productCreatorService;

    public CreateProductHandler(IProductCreatorService productCreatorService)
    {
        _productCreatorService = productCreatorService;
    }

    public async Task<HttpResultResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var createdProductId = await _productCreatorService.AddNewProduct(
              request.Title,
              request.Description,
              request.Status,
              request.Price,
              cancellationToken);

        return new HttpResultResponse()
        {
            Body = new ProductAck { Id = createdProductId },
            StatusCode = HttpStatusCode.OK,
            ContentType = MediaTypeNames.Application.Json,
        };
    }
}
