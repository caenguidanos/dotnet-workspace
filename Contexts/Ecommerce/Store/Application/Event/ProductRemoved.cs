namespace Ecommerce.Store.Application.Event;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Repository;
using Ecommerce.Store.Domain.ValueObject;

public class ProductRemovedEvent : INotification
{
    public Guid Product { get; set; }
}

public class ProductRemovedHandler : INotificationHandler<ProductRemovedEvent>
{
    private readonly IProductRepository _productRepository;

    public ProductRemovedHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ProductRemovedEvent notification, CancellationToken cancellationToken)
    {
        var newProductEventId = ProductEvent.NewID();

        var newProductEvent = new ProductEvent(
            new ProductEventId(newProductEventId),
            new ProductId(notification.Product),
            new ProductEventName("ecommerce_store_product_removed"));

        await _productRepository.SaveEvent(newProductEvent, cancellationToken);
    }
}
