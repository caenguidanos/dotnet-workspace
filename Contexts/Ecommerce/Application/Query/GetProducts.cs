namespace Ecommerce.Application.Query;

using MediatR;
using System.Net;
using System.Net.Mime;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Repository;

public readonly struct GetProductsQuery : IRequest<HttpResultResponse>
{
}

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, HttpResultResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<HttpResultResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.Get(cancellationToken);

        return new HttpResultResponse()
        {
            Body = products.Select(product => product.ToPrimitives()),
            StatusCode = HttpStatusCode.OK,
            ContentType = MediaTypeNames.Application.Json,
        };
    }
}
