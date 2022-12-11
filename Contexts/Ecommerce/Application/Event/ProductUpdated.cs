namespace Ecommerce.Application.Event;

using MediatR;

public readonly struct ProductUpdatedEvent : INotification
{
    public required Guid Product { get; init; }
}

public sealed class ProductUpdatedHandler : INotificationHandler<ProductUpdatedEvent>
{
    public Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // do stuff...
        Console.WriteLine("ecommerce_product_created");

        return Task.CompletedTask;
    }
}
