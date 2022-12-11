namespace Ecommerce.Application.Event;

using MediatR;

public sealed class ProductCreatedEvent : INotification
{
    public required Guid Product { get; init; }
}

public sealed class ProductCreatedHandler : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        // do stuff...
        Console.WriteLine("ecommerce_product_created");

        return Task.CompletedTask;
    }
}
