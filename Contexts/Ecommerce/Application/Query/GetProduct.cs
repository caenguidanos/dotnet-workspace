namespace Ecommerce.Application.Query;

using MediatR;
using System.Net;
using System.Net.Mime;

using Common.Application.HttpUtil;

using Ecommerce.Domain.Repository;

public readonly struct GetProductQuery : IRequest<HttpResultResponse>
{
    public required Guid Id { get; init; }
}

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, HttpResultResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<HttpResultResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {

        var product = await _productRepository.GetById(request.Id, cancellationToken);

        return new HttpResultResponse()
        {
            Body = product.ToPrimitives(),
            StatusCode = HttpStatusCode.OK,
            ContentType = MediaTypeNames.Application.Json,
        };
    }
}
