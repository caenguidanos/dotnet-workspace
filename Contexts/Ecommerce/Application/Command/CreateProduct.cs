namespace Ecommerce.Application.Command;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Service;

public readonly struct CreateProductCommand : IRequest<Result<ResultUnit, ProblemDetailsException>>
{
    public required int Price { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Status { get; init; }
}

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<ResultUnit, ProblemDetailsException>>
{
    private readonly IProductCreatorService _productCreatorService;

    public CreateProductHandler(IProductCreatorService productCreatorService)
    {
        _productCreatorService = productCreatorService;
    }

    public async ValueTask<Result<ResultUnit, ProblemDetailsException>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _productCreatorService.AddNewProduct(
              request.Title,
              request.Description,
              request.Status,
              request.Price,
              cancellationToken);

        if (result.IsFaulted)
        {
            return new Result<ResultUnit, ProblemDetailsException>(result.Error);
        }

        return new Result<ResultUnit, ProblemDetailsException>();
    }
}
