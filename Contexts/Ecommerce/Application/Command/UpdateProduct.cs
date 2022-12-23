namespace Ecommerce.Application.Command;

using Mediator;
using Common.Domain;
using Ecommerce.Domain.Service;

public readonly struct UpdateProductCommand : IRequest<Result<ResultUnit, ProblemDetailsException>>
{
    public Guid Id { get; init; }
    public int? Price { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Status { get; init; }
}

public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<ResultUnit, ProblemDetailsException>>
{
    private readonly IProductUpdaterService _productUpdaterService;

    public UpdateProductHandler(IProductUpdaterService productUpdaterService)
    {
        _productUpdaterService = productUpdaterService;
    }

    public async ValueTask<Result<ResultUnit, ProblemDetailsException>> Handle(UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        return await _productUpdaterService.UpdateProduct(request.Id, request, cancellationToken);
    }
}