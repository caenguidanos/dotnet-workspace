namespace Ecommerce.Store.Application.Notification;

using Ecommerce.Store.Domain.Notification;

public class ProductCreatedNotificationHandler : INotificationHandler<ProductCreatedNotification>
{
    public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product created: {notification.Id}");

        return Task.CompletedTask;
    }
}
