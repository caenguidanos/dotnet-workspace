namespace Ecommerce.Application.Query;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Repository;

public readonly struct GetProductQuery : IRequest<OneOf<ProductPrimitives, ProblemDetailsException>>
{
    public required Guid Id { get; init; }
}

public sealed class GetProductQueryHandler : IRequestHandler<GetProductQuery, OneOf<ProductPrimitives, ProblemDetailsException>>
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<OneOf<ProductPrimitives, ProblemDetailsException>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetById(request.Id, cancellationToken);
    }
}