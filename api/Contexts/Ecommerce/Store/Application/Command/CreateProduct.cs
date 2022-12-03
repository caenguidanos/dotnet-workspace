using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Service;
using api.Contexts.Ecommerce.Store.Domain.Event;

namespace api.Contexts.Ecommerce.Store.Application.Command
{
    public class CreateProductCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IPublisher _publisher;
        private readonly IProductService _productService;

        public CreateProductCommandHandler(IPublisher publisher, IProductService productService)
        {
            _publisher = publisher;
            _productService = productService;
        }

        public Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productCreatedEvent = new ProductCreatedEvent { Id = 1 };
            _publisher.Publish<ProductCreatedEvent>(productCreatedEvent);

            return Task.FromResult(1);
        }
    }
}