namespace Ecommerce.Application.Event;

using Mediator;

public readonly struct ProductCreatedEvent : INotification
{
    public required Guid Product { get; init; }
}

public sealed class ProductCreatedHandler : INotificationHandler<ProductCreatedEvent>
{
    public ValueTask Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        // do stuff...
        Console.WriteLine("ecommerce_product_created");

        return ValueTask.CompletedTask;
    }
}
