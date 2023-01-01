namespace Common.Core;

public static class HttpStatusText
{
    private static readonly System.Type HttpStatusCodeType = typeof(System.Net.HttpStatusCode);

    public static string From<TStatus>(in TStatus statusCode) where TStatus : IConvertible
    {
        return Enum.GetName(HttpStatusCodeType, statusCode) ?? string.Empty;
    }
}