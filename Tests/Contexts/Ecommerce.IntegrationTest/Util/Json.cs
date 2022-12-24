namespace Ecommerce.IntegrationTest.Util;

using System.Text.RegularExpressions;

public static partial class JsonUtil
{
    public static string MinifyString(string src)
    {
        return SearchSpacesRegExp().Replace(src, "$1");
    }

    [GeneratedRegex("(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+")]
    private static partial Regex SearchSpacesRegExp();
}