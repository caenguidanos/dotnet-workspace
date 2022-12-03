using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Entity;
using api.Contexts.Ecommerce.Store.Domain.Model;
using api.Contexts.Ecommerce.Store.Domain.Service;

namespace api.Contexts.Ecommerce.Store.Application.Query
{
    public class GetProductQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
    {
        private readonly IProductService _productService;

        public GetProductQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = _productService.Get(request.Id);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }

            return Task.FromResult(product);
        }
    }
}