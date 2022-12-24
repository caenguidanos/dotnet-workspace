namespace Ecommerce.Application;

using Mediator;

using Common.Domain;

using Domain;

using OneOf;

public readonly struct UpdateProductCommand : IRequest<OneOf<byte, ProblemDetailsException>>
{
    public Guid Id { get; init; }
    public int? Price { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Status { get; init; }
}

public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, OneOf<byte, ProblemDetailsException>>
{
    private readonly IProductUpdaterService _productUpdaterService;

    public UpdateProductHandler(IProductUpdaterService productUpdaterService)
    {
        _productUpdaterService = productUpdaterService;
    }

    public async ValueTask<OneOf<byte, ProblemDetailsException>> Handle(UpdateProductCommand request, CancellationToken cancellationToken) =>
        await _productUpdaterService.UpdateProduct(request.Id, request, cancellationToken);
}