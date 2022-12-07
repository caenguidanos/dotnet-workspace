namespace Ecommerce.Store.Application.Notification;

using Ecommerce.Store.Domain.Notification;

public class ProductUpdatedNotificationHandler : INotificationHandler<ProductUpdatedNotification>
{
    public Task Handle(ProductUpdatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product updated: {notification.Id}");

        return Task.CompletedTask;
    }
}
