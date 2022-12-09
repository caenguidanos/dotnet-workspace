namespace Ecommerce.Application.Event;

using MediatR;

public class ProductUpdatedEvent : INotification
{
    public Guid Product { get; set; }
}

public class ProductUpdatedHandler : INotificationHandler<ProductUpdatedEvent>
{
    public Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // do stuff...
        Console.WriteLine("ecommerce_product_created");

        return Task.CompletedTask;
    }
}
