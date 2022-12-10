namespace Ecommerce.Application.Event;

using MediatR;

public class ProductUpdatedEvent : INotification
{
    public required Guid Product { get; init; }
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
