namespace Ecommerce.Store.Application.Notification;

using Ecommerce.Store.Domain.Notification;

public class ProductCreatedSubscriber : INotificationHandler<ProductCreatedNotification>
{
    public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product created with id={notification.Id}");

        return Task.CompletedTask;
    }
}
