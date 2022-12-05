namespace Ecommerce.Store.Application.Query;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Repository;

public class GetByIdQuery : IRequest<Product>
{
    public required Guid Id { get; set; }
}

public class GetByIdHandler : IRequestHandler<GetByIdQuery, Product>
{
    private readonly IProductRepository productRepository;

    public GetByIdHandler(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<Product> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetById(request.Id, cancellationToken);

        return product;
    }
}
