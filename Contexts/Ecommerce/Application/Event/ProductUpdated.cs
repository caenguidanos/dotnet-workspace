namespace Ecommerce.Application;

public readonly struct ProductUpdatedEvent : INotification
{
    public required Guid Product { get; init; }
}

public sealed class ProductUpdatedHandler : INotificationHandler<ProductUpdatedEvent>
{
    public ValueTask Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // do stuff...
        Console.WriteLine("ecommerce_product_updated");

        return ValueTask.CompletedTask;
    }
}