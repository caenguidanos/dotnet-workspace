namespace Common.Core;

public static partial class Json
{
    public static string MinifyString(in string src)
    {
        return SearchSpacesRegExp().Replace(src, "$1");
    }

    public static readonly JsonSerializerOptions OutHttpJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    [GeneratedRegex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+")]
    private static partial Regex SearchSpacesRegExp();
}