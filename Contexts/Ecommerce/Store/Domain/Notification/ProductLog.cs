namespace Ecommerce.Store.Domain.Notification;

public class ProductLog
{
    public const int ControllerGetAllNotImplemented = 1000;
    public const int ControllerGetByIdNotFound = 1001;
    public const int ControllerGetByIdNotImplemented = 1002;
    public const int ControllerCreateBadRequest = 1003;
    public const int ControllerCreateNotImplemented = 1004;
    public const int ControllerDeleteByIdNotFound = 1005;
    public const int ControllerDeleteByIdNotImplemented = 1006;
}


public class ProductLogNotification : INotification
{
    public required int Event { get; set; }
    public required string Message { get; set; }
}
