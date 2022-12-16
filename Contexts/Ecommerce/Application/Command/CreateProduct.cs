namespace Ecommerce.Application.Command;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Error;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct CreateProductCommand : IRequest<Result<ProductAck, ProductError>>
{
    public required int Price { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Status { get; init; }
}

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<ProductAck, ProductError>>
{
    private readonly IProductCreatorService _productCreatorService;

    public CreateProductHandler(IProductCreatorService productCreatorService)
    {
        _productCreatorService = productCreatorService;
    }

    public async ValueTask<Result<ProductAck, ProductError>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await _productCreatorService.AddNewProduct(
              request.Title,
              request.Description,
              request.Status,
              request.Price,
              cancellationToken);

        if (result.IsFaulted)
        {
            return new Result<ProductAck, ProductError>(result.Err);
        }

        return new Result<ProductAck, ProductError>(new ProductAck { Id = result.Ok });
    }
}
