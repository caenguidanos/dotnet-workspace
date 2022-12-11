namespace Ecommerce.Application.Query;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mime;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Repository;

public readonly struct GetProductQuery : IRequest<HttpResultResponse>
{
    public required Guid Id { get; init; }
}

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, HttpResultResponse>
{
    private readonly ILogger _logger;
    private readonly IProductRepository _productRepository;

    public GetProductHandler(ILogger<GetProductHandler> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<HttpResultResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productRepository.GetById(request.Id, cancellationToken);

            return new HttpResultResponse(cancellationToken)
            {
                Body = product.ToPrimitives(),
                StatusCode = HttpStatusCode.OK,
                ContentType = MediaTypeNames.Application.Json,
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
