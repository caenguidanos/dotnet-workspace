namespace Ecommerce.Store.Application.Notification;

using Ecommerce.Store.Domain.Notification;

public class ProductControllerCreatedNotificationHandler : INotificationHandler<ProductControllerCreatedNotification>
{
    public Task Handle(ProductControllerCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product created: {notification.Id}");

        return Task.CompletedTask;
    }
}
