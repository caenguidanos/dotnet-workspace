using MediatR;
using api.Contexts.Ecommerce.Store.Domain.Service;
using api.Contexts.Ecommerce.Store.Domain.Event;

namespace api.Contexts.Ecommerce.Store.Application.Event
{
    public class ProductRemovedEventHandler : INotificationHandler<ProductRemovedEvent>
    {
        private readonly IProductService _productService;

        public ProductRemovedEventHandler(IProductService productService)
        {
            _productService = productService;
        }

        public Task Handle(ProductRemovedEvent notification, CancellationToken cancellationToken)
        {
            System.Console.WriteLine($"Product removed with id={notification.Id}");

            return Task.CompletedTask;
        }
    }
}