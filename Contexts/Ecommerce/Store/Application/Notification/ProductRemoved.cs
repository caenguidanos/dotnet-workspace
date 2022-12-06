namespace Ecommerce.Store.Application.Notification;

using Ecommerce.Store.Domain.LogEvent;
using Ecommerce.Store.Domain.Notification;
using Microsoft.Extensions.Logging;

public class ProductRemovedNotificationHandler : INotificationHandler<ProductRemovedNotification>
{
    private readonly ILogger _logger;

    public ProductRemovedNotificationHandler(ILogger<ProductCreatedNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductRemovedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(ProductLogEvent.CreatedNotification, $"Product removed: {notification.Id}");

        return Task.CompletedTask;
    }
}
