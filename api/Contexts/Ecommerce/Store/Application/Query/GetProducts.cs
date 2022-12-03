using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Repository;

namespace api.Contexts.Ecommerce.Store.Application.Query
{
    public class GetProductsQuery : IRequest<IEnumerable<Product>> { }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<IEnumerable<Product>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var product = _productRepository.GetAll();

            return Task.FromResult(product);
        }
    }
}