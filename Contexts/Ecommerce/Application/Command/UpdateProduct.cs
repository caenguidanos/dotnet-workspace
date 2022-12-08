namespace Ecommerce.Application.Command;

using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.DataTransfer;

public class UpdateProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public int? Price { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
}

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductService productService;

    public UpdateProductHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new ProductPrimitivesForUpdateOperation
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            Price = request.Price,
        };

        await productService.UpdateProduct(request.Id, product, cancellationToken);

        return Unit.Value;
    }
}
