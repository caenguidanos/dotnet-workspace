namespace Ecommerce.Application.Query;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Repository;

public class GetProductsQuery : IRequest<IEnumerable<Product>>
{
}

public class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    private readonly IProductRepository productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.Get(cancellationToken);

        return products;
    }
}