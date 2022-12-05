namespace Ecommerce.Store.Application.Query;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Repository;

public class GetAllQuery : IRequest<IEnumerable<Product>>
{
}

public class GetAllHandler : IRequestHandler<GetAllQuery, IEnumerable<Product>>
{
    private readonly IProductRepository productRepository;

    public GetAllHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAll(cancellationToken);

        return products;
    }
}
