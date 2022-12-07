namespace Ecommerce.Store.Application.Command;

using Ecommerce.Store.Domain.Service;
using Ecommerce.Store.Infrastructure.DataTransfer;

public class UpdateProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public int? Price { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductService productService;

    public UpdateProductCommandHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        await productService.UpdateProductById(
            request.Id,
            new ProductPrimitivesForUpdateOperation
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Price = request.Price,
            },
            cancellationToken);

        return Unit.Value;
    }
}
