namespace Ecommerce.Application.Command;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Service;

public readonly struct UpdateProductCommand : IRequest<HttpResultResponse>
{
    public Guid Id { get; init; }
    public int? Price { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Status { get; init; }
}

public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, HttpResultResponse>
{
    private readonly ILogger _logger;
    private readonly IProductUpdaterService _productUpdaterService;

    public UpdateProductHandler(ILogger<UpdateProductHandler> logger, IProductUpdaterService productUpdaterService)
    {
        _logger = logger;
        _productUpdaterService = productUpdaterService;
    }

    public async Task<HttpResultResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _productUpdaterService.UpdateProduct(request.Id, request, cancellationToken);

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
