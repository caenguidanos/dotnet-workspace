namespace Ecommerce.Application.Command;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Error;
using Ecommerce.Domain.Service;

public readonly struct RemoveProductCommand : IRequest<Result<byte, ProductError>>
{
    public required Guid Id { get; init; }
}

public sealed class RemoveProductHandler : IRequestHandler<RemoveProductCommand, Result<byte, ProductError>>
{
    private readonly IProductRemoverService _productRemoverService;

    public RemoveProductHandler(IProductRemoverService productRemoverService)
    {
        _productRemoverService = productRemoverService;
    }

    public async ValueTask<Result<byte, ProductError>> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        return await _productRemoverService.RemoveProduct(request.Id, cancellationToken);
    }
}
