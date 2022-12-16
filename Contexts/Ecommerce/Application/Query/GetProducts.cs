namespace Ecommerce.Application.Query;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Error;
using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct GetProductsQuery : IRequest<Result<IEnumerable<ProductPrimitives>, ProductError>>
{
}

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<ProductPrimitives>, ProductError>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<Result<IEnumerable<ProductPrimitives>, ProductError>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.Get(cancellationToken);
        if (result.IsFaulted)
        {
            return new Result<IEnumerable<ProductPrimitives>, ProductError>(result.Err);
        }

        return new Result<IEnumerable<ProductPrimitives>, ProductError>(result.Ok.Select(product => product.ToPrimitives()));
    }
}
