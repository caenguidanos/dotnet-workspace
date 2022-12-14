namespace Ecommerce.Application.Query;

using Mediator;

using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;
using Common.Domain;

public readonly struct GetProductQuery : IRequest<Result<ProductPrimitives>>
{
    public required Guid Id { get; init; }
}

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, Result<ProductPrimitives>>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<Result<ProductPrimitives>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {

        var result = await _productRepository.GetById(request.Id, cancellationToken);
        if (result.Err is not null)
        {
            return new Result<ProductPrimitives>(null, result.Err);
        }

        if (result.Ok is null)
        {
            throw new ArgumentNullException();
        }

        return new Result<ProductPrimitives>(result.Ok.ToPrimitives());
    }
}
