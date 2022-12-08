namespace Ecommerce.Application.Command;

using Ecommerce.Domain.Service;

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
        await productService.UpdateProduct(request.Id, request, cancellationToken);

        return Unit.Value;
    }
}
