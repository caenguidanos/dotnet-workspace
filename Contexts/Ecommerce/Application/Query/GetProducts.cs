namespace Ecommerce.Application.Query;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct GetProductsQuery : IRequest<Result<IEnumerable<ProductPrimitives>, ProblemDetailsException>>
{
}

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<ProductPrimitives>, ProblemDetailsException>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<Result<IEnumerable<ProductPrimitives>, ProblemDetailsException>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.Get(cancellationToken);
        if (result.IsFaulted)
        {
            return new Result<IEnumerable<ProductPrimitives>, ProblemDetailsException>(result.Error);
        }

        return new Result<IEnumerable<ProductPrimitives>, ProblemDetailsException>(result.Value.Select(product => product.ToPrimitives()));
    }
}
