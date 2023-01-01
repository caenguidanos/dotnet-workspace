namespace Ecommerce.Application.Command;

using Ecommerce.Domain.Service;

public readonly struct RemoveProductCommand : IRequest<OneOf<byte, ProblemDetailsException>>
{
    public required Guid Id { get; init; }
}

public sealed class RemoveProductHandler : IRequestHandler<RemoveProductCommand, OneOf<byte, ProblemDetailsException>>
{
    private readonly IProductRemoverService _productRemoverService;

    public RemoveProductHandler(IProductRemoverService productRemoverService)
    {
        _productRemoverService = productRemoverService;
    }

    public async ValueTask<OneOf<byte, ProblemDetailsException>> Handle(RemoveProductCommand request, CancellationToken cancellationToken) =>
        await _productRemoverService.RemoveProduct(request.Id, cancellationToken);
}