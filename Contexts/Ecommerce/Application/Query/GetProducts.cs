namespace Ecommerce.Application.Query;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mime;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Repository;

public readonly struct GetProductsQuery : IRequest<HttpResultResponse>
{
}

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, HttpResultResponse>
{
    private readonly ILogger _logger;
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(ILogger<GetProductsHandler> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<HttpResultResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productRepository.Get(cancellationToken);

            return new HttpResultResponse(cancellationToken)
            {
                Body = products.Select(product => product.ToPrimitives()),
                StatusCode = HttpStatusCode.OK,
                ContentType = MediaTypeNames.Application.Json,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());

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
