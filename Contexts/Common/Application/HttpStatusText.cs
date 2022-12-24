namespace Common.Application;

public static class HttpStatusText
{
    public static string From<TStatus>(TStatus statusCode) where TStatus : IConvertible
    {
        return Enum.GetName(typeof(System.Net.HttpStatusCode), statusCode) ?? string.Empty;
    }
}