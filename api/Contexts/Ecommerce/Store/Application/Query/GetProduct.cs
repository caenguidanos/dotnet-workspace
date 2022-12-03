using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Repository;

namespace api.Contexts.Ecommerce.Store.Application.Query
{
    public class GetProductQuery : IRequest<Product>
    {
        public required string Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<Product> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            var product = _productRepository.Get(query.Id);

            return Task.FromResult(product);
        }
    }
}