namespace Ecommerce.Application.Command;

using Mediator;

using Common.Domain;

using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct CreateProductCommand : IRequest<Result<ProductAck?>>
{
    public required int Price { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Status { get; init; }
}

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<ProductAck?>>
{
    private readonly IProductCreatorService _productCreatorService;

    public CreateProductHandler(IProductCreatorService productCreatorService)
    {
        _productCreatorService = productCreatorService;
    }

    public async ValueTask<Result<ProductAck?>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await _productCreatorService.AddNewProduct(
              request.Title,
              request.Description,
              request.Status,
              request.Price,
              cancellationToken);

        if (result.Err is not null)
        {
            return new Result<ProductAck?>(null, result.Err);
        }

        if (result.Ok is null)
        {
            throw new ArgumentNullException();
        }

        var ack = new ProductAck { Id = (Guid)result.Ok };

        return new Result<ProductAck?>(ack);
    }
}
