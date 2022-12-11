namespace Ecommerce.Application.Query;

using MediatR;
using System.Net;
using System.Net.Mime;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Repository;

public sealed class GetProductsQuery : IRequest<HttpResultResponse>
{
}

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, HttpResultResponse>
{
    private readonly IProductRepository productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<HttpResultResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await productRepository.Get(cancellationToken);

            return new HttpResultResponse(cancellationToken)
            {
                Body = products.Select(product => product.ToPrimitives()),
                StatusCode = HttpStatusCode.OK,
                ContentType = MediaTypeNames.Application.Json,
            };
        }
        catch (Exception ex)
        {
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
