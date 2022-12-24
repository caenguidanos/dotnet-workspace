namespace Ecommerce.Application;

using Mediator;

using Common.Domain;

using Domain;

using Infrastructure;

using OneOf;

public readonly struct GetProductsQuery : IRequest<OneOf<List<ProductPrimitives>, ProblemDetailsException>>
{
}

public sealed class
    GetProductsHandler : IRequestHandler<GetProductsQuery, OneOf<List<ProductPrimitives>, ProblemDetailsException>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<OneOf<List<ProductPrimitives>, ProblemDetailsException>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.Get(cancellationToken);

        return result.Match<OneOf<List<ProductPrimitives>, ProblemDetailsException>>(
            products => products.ConvertAll(product => product.ToPrimitives()),
            exception => exception
        );
    }
}