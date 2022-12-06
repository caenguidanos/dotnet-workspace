namespace Ecommerce.Store.Application.Command;

using Ecommerce.Store.Domain.Service;

public class CreateProductCommand : IRequest<Unit>
{
    public required int Price { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Status { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Unit>
{
    private readonly IProductService productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await productService.AddNewProduct(
            request.Title,
            request.Description,
            request.Status,
            request.Price,
            cancellationToken);

        return Unit.Value;
    }
}
