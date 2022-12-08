namespace Contexts.Ecommerce.Application.Command;

using Contexts.Ecommerce.Domain.Service;
using Contexts.Ecommerce.Infrastructure.DataTransfer;

public class CreateProductCommand : IRequest<ProductAck>
{
    public required int Price { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Status { get; set; }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductAck>
{
    private readonly IProductService productService;

    public CreateProductHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<ProductAck> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var createdProductId = await productService.AddNewProduct(
            request.Title,
            request.Description,
            request.Status,
            request.Price,
            cancellationToken);

        return new ProductAck { Id = createdProductId };
    }
}
