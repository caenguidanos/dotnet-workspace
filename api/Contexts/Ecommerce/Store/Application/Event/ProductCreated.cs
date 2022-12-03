using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Service;
using api.Contexts.Ecommerce.Store.Domain.Event;

namespace api.Contexts.Ecommerce.Store.Application.Event
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly IProductService _productService;

        public ProductCreatedEventHandler(IProductService productService)
        {
            _productService = productService;
        }

        public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            System.Console.WriteLine($"Product created with id={notification.Id}");

            return Task.CompletedTask;
        }
    }
}