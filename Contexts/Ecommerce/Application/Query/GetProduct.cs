namespace Ecommerce.Application.Query;

using Mediator;
using Common.Domain;
using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;
using OneOf;

public readonly struct GetProductQuery : IRequest<OneOf<ProductPrimitives, ProblemDetailsException>>
{
    public required Guid Id { get; init; }
}

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, OneOf<ProductPrimitives, ProblemDetailsException>>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<OneOf<ProductPrimitives, ProblemDetailsException>> Handle(GetProductQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetById(request.Id, cancellationToken);

        return result.Match<OneOf<ProductPrimitives, ProblemDetailsException>>(
            product => product.ToPrimitives(),
            exception => exception
        );
    }
}