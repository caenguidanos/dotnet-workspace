namespace Ecommerce.Store.Application.Notification;

using Ecommerce.Store.Domain.Notification;

public class ProductLogNotificationHandler : INotificationHandler<ProductLogNotification>
{
    public Task Handle(ProductLogNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product::Log::{notification.Event} - {notification.Message}");

        return Task.CompletedTask;
    }
}
