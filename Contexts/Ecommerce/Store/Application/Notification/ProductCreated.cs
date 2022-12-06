namespace Ecommerce.Store.Application.Notification;

using Ecommerce.Store.Domain.LogEvent;
using Ecommerce.Store.Domain.Notification;
using Microsoft.Extensions.Logging;

public class ProductCreatedNotificationHandler : INotificationHandler<ProductCreatedNotification>
{
    private readonly ILogger _logger;

    public ProductCreatedNotificationHandler(ILogger<ProductCreatedNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(ProductLogEvent.CreatedNotification, $"Product created: {notification.Id}");

        return Task.CompletedTask;
    }
}
