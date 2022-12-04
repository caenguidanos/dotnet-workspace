using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Repository;

namespace api.Contexts.Ecommerce.Store.Application.Query
{
    public class GetByIdQuery : IRequest<Product>
    {
        public required Guid Id { get; set; }
    }

    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(GetByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(query.Id, cancellationToken);

            return product;
        }
    }
}