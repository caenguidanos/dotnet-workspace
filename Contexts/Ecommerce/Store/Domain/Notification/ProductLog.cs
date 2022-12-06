namespace Ecommerce.Store.Domain.Notification;

public class ProductLog
{
    public const int GetAllNotImplemented = 1000;
    public const int GetByIdNotFound = 1001;
    public const int GetByIdNotImplemented = 1002;
    public const int CreateBadRequest = 1003;
    public const int CreateNotImplemented = 1004;
    public const int DeleteByIdNotFound = 1005;
    public const int DeleteByIdNotImplemented = 1006;
    public const int CreatedNotification = 1007;
    public const int DeletedNotification = 1008;
}


public class ProductLogNotification : INotification
{
    public required int Event { get; set; }
    public required string Message { get; set; }
}
