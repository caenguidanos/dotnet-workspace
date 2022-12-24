namespace Common.Application;

public static partial class Json
{
    public static string MinifyString(string src)
    {
        return SearchSpacesRegExp().Replace(src, "$1");
    }

    [GeneratedRegex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+")]
    private static partial Regex SearchSpacesRegExp();
}