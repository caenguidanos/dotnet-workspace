namespace Ecommerce.Application.Query;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct GetProductsQuery : IRequest<Result<IEnumerable<ProductPrimitives>>>
{
}

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<ProductPrimitives>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<Result<IEnumerable<ProductPrimitives>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.Get(cancellationToken);

        if (result.Err is not null)
        {
            return new Result<IEnumerable<ProductPrimitives>> { Err = result.Err };
        }

        return new Result<IEnumerable<ProductPrimitives>>
        {
            Ok = result.Ok.Select(product => product.ToPrimitives()),
        };
    }
}
