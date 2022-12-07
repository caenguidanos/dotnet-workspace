namespace Ecommerce.Store.Application.Command;

using Ecommerce.Store.Domain.Service;
using Ecommerce.Store.Infrastructure.DataTransfer;

public class CreateProductCommand : IRequest<ProductAck>
{
    public required int Price { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Status { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductAck>
{
    private readonly IProductService productService;

    public CreateProductCommandHandler(IProductService productService)
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
