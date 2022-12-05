namespace Ecommerce.Store.Application.Command;

using Ecommerce.Store.Domain.Service;

public class CreateProductCommand : IRequest<Guid>
{
    public required int Price { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required int Status { get; set; }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductService productService;

    public CreateProductHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var id = await productService.AddNewProduct(
            request.Title,
            request.Description,
            request.Status,
            request.Price,
            cancellationToken);

        return id;
    }
}
