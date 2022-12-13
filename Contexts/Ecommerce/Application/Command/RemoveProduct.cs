namespace Ecommerce.Application.Command;

using Mediator;

using Ecommerce.Domain.Service;

public readonly struct RemoveProductCommand : IRequest<Unit>
{
    public required Guid Id { get; init; }
}

public sealed class RemoveProductHandler : IRequestHandler<RemoveProductCommand, Unit>
{
    private readonly IProductRemoverService _productRemoverService;

    public RemoveProductHandler(IProductRemoverService productRemoverService)
    {
        _productRemoverService = productRemoverService;
    }

    public async ValueTask<Unit> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        await _productRemoverService.RemoveProduct(request.Id, cancellationToken);

        return Unit.Value;
    }
}
