namespace Ecommerce.Application.Command;

using Mediator;

using Ecommerce.Domain.Service;
using Common.Domain;

public readonly struct RemoveProductCommand : IRequest<Result<bool>>
{
    public required Guid Id { get; init; }
}

public sealed class RemoveProductHandler : IRequestHandler<RemoveProductCommand, Result<bool>>
{
    private readonly IProductRemoverService _productRemoverService;

    public RemoveProductHandler(IProductRemoverService productRemoverService)
    {
        _productRemoverService = productRemoverService;
    }

    public async ValueTask<Result<bool>> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        return await _productRemoverService.RemoveProduct(request.Id, cancellationToken);
    }
}
