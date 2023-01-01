namespace Ecommerce.Application.Query;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Repository;

public readonly struct GetProductsQuery : IRequest<OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>>
{
}

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.Get(cancellationToken);
    }
}