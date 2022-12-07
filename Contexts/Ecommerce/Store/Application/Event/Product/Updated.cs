namespace Ecommerce.Store.Application.Event;

using Ecommerce.Store.Domain.Entity;
using Ecommerce.Store.Domain.Repository;
using Ecommerce.Store.Domain.ValueObject;

public class ProductUpdatedEvent : INotification
{
    public Guid Product { get; set; }
}

public class ProductUpdatedHandler : INotificationHandler<ProductUpdatedEvent>
{
    private readonly IProductRepository _productRepository;

    public ProductUpdatedHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var newProductEventId = Common.Domain.Entity.Entity.NewID();

        var newProductEvent = new ProductEvent(
            new ProductEventId(newProductEventId),
            new ProductId(notification.Product),
            new ProductEventName("ecommerce_store_product_updated"));

        await _productRepository.SaveEvent(newProductEvent, cancellationToken);
    }
}
