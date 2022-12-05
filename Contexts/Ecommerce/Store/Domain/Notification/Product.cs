namespace Ecommerce.Store.Domain.Notification;

public class ProductCreatedNotification : INotification
{
    public required Guid Id { get; set; }
}

public class ProductRemovedNotification : INotification
{
    public required Guid Id { get; set; }
}
