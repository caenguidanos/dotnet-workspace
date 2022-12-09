namespace Ecommerce.Application.Event;

using MediatR;

public class ProductRemovedEvent : INotification
{
    public Guid Product { get; set; }
}

public class ProductRemovedHandler : INotificationHandler<ProductRemovedEvent>
{
    public Task Handle(ProductRemovedEvent notification, CancellationToken cancellationToken)
    {
        // do stuff...
        Console.WriteLine("ecommerce_product_removed");

        return Task.CompletedTask;
    }
}
