namespace Ecommerce.Application.Event;

public class ProductCreatedEvent : INotification
{
    public Guid Product { get; set; }
}

public class ProductCreatedHandler : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        // do stuff...
        Console.WriteLine("ecommerce_product_created");

        return Task.CompletedTask;
    }
}
