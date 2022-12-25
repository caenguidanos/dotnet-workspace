namespace Common.Application;

public static partial class Json
{
    public static string MinifyString(string src)
    {
        return SearchSpacesRegExp().Replace(src, "$1");
    }

    public static readonly JsonSerializerOptions HttpSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    [GeneratedRegex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+")]
    private static partial Regex SearchSpacesRegExp();
}