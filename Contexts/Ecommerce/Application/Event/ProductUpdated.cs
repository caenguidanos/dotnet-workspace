namespace Ecommerce.Application.Event;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.ValueObject;

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
        var newProductEventId = Common.Domain.Schema.NewID();

        var newProductEvent = new ProductEvent(
            new ProductEventId(newProductEventId),
            new ProductId(notification.Product),
            new ProductEventName("ecommerce_product_updated"));

        await _productRepository.SaveEvent(newProductEvent, cancellationToken);
    }
}
