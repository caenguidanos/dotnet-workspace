namespace Ecommerce.Store.Application.Query;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Repository;

public class GetProductsQuery : IRequest<IEnumerable<Product>>
{
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    private readonly IProductRepository productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAll(cancellationToken);

        return products;
    }
}
