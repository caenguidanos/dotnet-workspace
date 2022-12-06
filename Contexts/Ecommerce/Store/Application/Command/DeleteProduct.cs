namespace Ecommerce.Store.Application.Command;

using Ecommerce.Store.Domain.Service;

public class DeleteProductCommand : IRequest<Unit>
{
    public required Guid Id { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductService productService;

    public DeleteProductCommandHandler(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await productService.DeleteProductById(request.Id, cancellationToken);

        return Unit.Value;
    }
}
