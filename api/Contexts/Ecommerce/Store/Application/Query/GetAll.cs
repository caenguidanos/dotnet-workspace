using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Repository;

namespace api.Contexts.Ecommerce.Store.Application.Query
{
    public class GetAllQuery : IRequest<IEnumerable<Product>> { }

    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllQuery query, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAll(cancellationToken);

            return products;
        }
    }
}