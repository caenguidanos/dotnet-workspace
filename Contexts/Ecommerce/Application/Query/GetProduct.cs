namespace Ecommerce.Application.Query;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Repository;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct GetProductQuery : IRequest<Result<ProductPrimitives, ProblemDetailsException>>
{
    public required Guid Id { get; init; }
}

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, Result<ProductPrimitives, ProblemDetailsException>>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async ValueTask<Result<ProductPrimitives, ProblemDetailsException>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.GetById(request.Id, cancellationToken);
        if (result.IsFaulted)
        {
            return new Result<ProductPrimitives, ProblemDetailsException>(result.Error);
        }

        return new Result<ProductPrimitives, ProblemDetailsException>(result.Value.ToPrimitives());
    }
}
