namespace Ecommerce.Store.Application.Notification;

using Ecommerce.Store.Domain.Notification;

public class ProductRemovedSubscriber : INotificationHandler<ProductRemovedNotification>
{
    public Task Handle(ProductRemovedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product removed with id={notification.Id}");

        return Task.CompletedTask;
    }
}
