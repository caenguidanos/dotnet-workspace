namespace Ecommerce.Application.Query;

using Mediator;

using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct GetProductsQuery : IRequest<IEnumerable<ProductPrimitives>>
{
}

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductPrimitives>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<IEnumerable<ProductPrimitives>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.Get(cancellationToken);

        return products.Select(product => product.ToPrimitives());
    }
}
