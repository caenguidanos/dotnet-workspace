namespace Ecommerce.Application;

public readonly struct GetProductsQuery : IRequest<OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>>
{
}

public sealed class
    GetProductsHandler : IRequestHandler<GetProductsQuery, OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<OneOf<IEnumerable<ProductPrimitives>, ProblemDetailsException>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.Get(cancellationToken);
    }
}