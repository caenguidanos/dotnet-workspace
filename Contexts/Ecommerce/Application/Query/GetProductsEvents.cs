namespace Ecommerce.Application.Query;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Repository;

public class GetProductsEventsQuery : IRequest<IEnumerable<ProductEvent>>
{
}

public class GetProductsEventsHandler : IRequestHandler<GetProductsEventsQuery, IEnumerable<ProductEvent>>
{
    private readonly IProductRepository productRepository;

    public GetProductsEventsHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductEvent>> Handle(GetProductsEventsQuery request, CancellationToken cancellationToken)
    {
        var productsEvents = await productRepository.GetEvents(cancellationToken);

        return productsEvents;
    }
}
