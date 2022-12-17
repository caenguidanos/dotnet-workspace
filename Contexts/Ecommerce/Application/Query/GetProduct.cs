namespace Ecommerce.Application.Query;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Error;
using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct GetProductQuery : IRequest<Result<ProductPrimitives, ProductException>>
{
    public required Guid Id { get; init; }
}

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, Result<ProductPrimitives, ProductException>>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<Result<ProductPrimitives, ProductException>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetById(request.Id, cancellationToken);
        if (result.IsFaulted)
        {
            return new Result<ProductPrimitives, ProductException>(result.Error);
        }

        return new Result<ProductPrimitives, ProductException>(result.Value.ToPrimitives());
    }
}
