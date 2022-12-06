namespace Ecommerce.Store.Application.Query;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Repository;

public class GetProductQuery : IRequest<Product>
{
    public required Guid Id { get; set; }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
{
    private readonly IProductRepository productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetById(request.Id, cancellationToken);

        return product;
    }
}
