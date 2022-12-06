namespace Ecommerce.Store.Application.Notification;

using Ecommerce.Store.Domain.Notification;

public class ProductRemovedNotificationHandler : INotificationHandler<ProductRemovedNotification>
{
    public Task Handle(ProductRemovedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product removed: {notification.Id}");

        return Task.CompletedTask;
    }
}
