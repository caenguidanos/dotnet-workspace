namespace Ecommerce.Application.Command;

using Mediator;

using Common.Application;

using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.DataTransfer;

public readonly struct CreateProductCommand : IRequest<ProductAck>
{
    public required int Price { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Status { get; init; }
}

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductAck>
{
    private readonly IProductCreatorService _productCreatorService;

    public CreateProductHandler(IProductCreatorService productCreatorService)
    {
        _productCreatorService = productCreatorService;
    }

    public async ValueTask<ProductAck> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var createdProductId = await _productCreatorService.AddNewProduct(
              request.Title,
              request.Description,
              request.Status,
              request.Price,
              cancellationToken);

        return new ProductAck { Id = createdProductId };
    }
}
