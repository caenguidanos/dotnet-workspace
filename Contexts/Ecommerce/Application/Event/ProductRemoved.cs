namespace Ecommerce.Application.Event;

using MediatR;

public class ProductRemovedEvent : INotification
{
    public required Guid Product { get; init; }
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
