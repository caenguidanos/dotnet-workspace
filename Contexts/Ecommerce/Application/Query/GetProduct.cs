namespace Ecommerce.Application.Query;

using Mediator;

using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct GetProductQuery : IRequest<ProductPrimitives>
{
    public required Guid Id { get; init; }
}

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, ProductPrimitives>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<ProductPrimitives> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {

        var product = await _productRepository.GetById(request.Id, cancellationToken);

        return product.ToPrimitives();
    }
}
