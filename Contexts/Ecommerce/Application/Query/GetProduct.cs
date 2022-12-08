namespace Ecommerce.Application.Query;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Repository;

public class GetProductQuery : IRequest<Product>
{
    public required Guid Id { get; set; }
}

public class GetProductHandler : IRequestHandler<GetProductQuery, Product>
{
    private readonly IProductRepository productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetById(request.Id, cancellationToken);

        return product;
    }
}
