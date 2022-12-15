namespace Ecommerce.Application.Query;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;

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
            return new Result<ProductPrimitives> { Err = result.Err };
        }

        return new Result<ProductPrimitives> { Ok = result.Ok.ToPrimitives() };
    }
}
