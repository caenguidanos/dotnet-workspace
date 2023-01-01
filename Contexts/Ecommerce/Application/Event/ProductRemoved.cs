namespace Ecommerce.Application.Event;

public readonly struct ProductRemovedEvent : INotification
{
    public required Guid Product { get; init; }
}

public sealed class ProductRemovedHandler : INotificationHandler<ProductRemovedEvent>
{
    public ValueTask Handle(ProductRemovedEvent notification, CancellationToken cancellationToken)
    {
        // do stuff...
        Console.WriteLine("ecommerce_product_removed");

        return ValueTask.CompletedTask;
    }
}