namespace Ecommerce.Application.Command;

using Mediator;
using System.Net;

using Common.Application;

using Ecommerce.Domain.Service;

public readonly struct UpdateProductCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public int? Price { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Status { get; init; }
}

public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductUpdaterService _productUpdaterService;

    public UpdateProductHandler(IProductUpdaterService productUpdaterService)
    {
        _productUpdaterService = productUpdaterService;
    }

    public async ValueTask<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        await _productUpdaterService.UpdateProduct(request.Id, request, cancellationToken);

        return Unit.Value;
    }
}
