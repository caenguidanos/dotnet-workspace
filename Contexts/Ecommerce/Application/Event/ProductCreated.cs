namespace Ecommerce.Application.Event;

using Ecommerce.Domain.Entity;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.ValueObject;

public class ProductCreatedEvent : INotification
{
    public Guid Product { get; set; }
}

public class ProductCreatedHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IProductRepository _productRepository;

    public ProductCreatedHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        var newProductEventId = Product.NewID();

        var newProductEvent = new ProductEvent(
            new ProductEventId(newProductEventId),
            new ProductId(notification.Product),
            new ProductEventName("ecommerce_store_product_created"));

        await _productRepository.SaveEvent(newProductEvent, cancellationToken);
    }
}
