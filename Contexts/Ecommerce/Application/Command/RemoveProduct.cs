namespace Ecommerce.Application.Command;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct RemoveProductCommand : IRequest<Result<ProductAck, ProblemDetailsException>>
{
    public required Guid Id { get; init; }
}

public sealed class RemoveProductHandler : IRequestHandler<RemoveProductCommand, Result<ProductAck, ProblemDetailsException>>
{
    private readonly IProductRemoverService _productRemoverService;

    public RemoveProductHandler(IProductRemoverService productRemoverService)
    {
        _productRemoverService = productRemoverService;
    }

    public async ValueTask<Result<ProductAck, ProblemDetailsException>> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        return await _productRemoverService.RemoveProduct(request.Id, cancellationToken);
    }
}
