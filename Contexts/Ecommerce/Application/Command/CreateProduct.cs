namespace Ecommerce.Application.Command;

using Ecommerce.Domain.Service;

public readonly struct CreateProductCommand : IRequest<OneOf<byte, ProblemDetailsException>>
{
    public required Guid Id { get; init; }
    public required int Price { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Status { get; init; }
}

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, OneOf<byte, ProblemDetailsException>>
{
    private readonly IProductCreatorService _productCreatorService;

    public CreateProductCommandHandler(IProductCreatorService productCreatorService)
    {
        _productCreatorService = productCreatorService;
    }

    public async ValueTask<OneOf<byte, ProblemDetailsException>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await _productCreatorService.AddNewProduct(
            request.Id,
            request.Title,
            request.Description,
            request.Status,
            request.Price,
            cancellationToken);

        return result.Match<OneOf<byte, ProblemDetailsException>>(_ => default, error => error);
    }
}