namespace Ecommerce.Application.Query;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Error;
using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct GetProductQuery : IRequest<Result<ProductPrimitives, ProductError>>
{
    public required Guid Id { get; init; }
}

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, Result<ProductPrimitives, ProductError>>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<Result<ProductPrimitives, ProductError>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetById(request.Id, cancellationToken);
        if (result.IsFaulted)
        {
            return new Result<ProductPrimitives, ProductError>(result.Err);
        }

        return new Result<ProductPrimitives, ProductError>(result.Ok.ToPrimitives());
    }
}
