namespace Ecommerce.Application.Command;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Service;

public readonly struct DeleteProductCommand : IRequest<HttpResultResponse>
{
    public required Guid Id { get; init; }
}

public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand, HttpResultResponse>
{
    private readonly ILogger _logger;
    private readonly IProductService _productService;

    public DeleteProductHandler(ILogger<DeleteProductHandler> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<HttpResultResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _productService.DeleteProduct(request.Id, cancellationToken);

            return new HttpResultResponse(cancellationToken)
            {
                StatusCode = HttpStatusCode.Accepted,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

            if (ex is ProductNotFoundException)
            {
                return new HttpResultResponse(cancellationToken)
                {
                    StatusCode = HttpStatusCode.NotFound,
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
