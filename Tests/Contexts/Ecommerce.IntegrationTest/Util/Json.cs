namespace Ecommerce.IntegrationTest.Util;

using System.Text.RegularExpressions;

public static class JsonUtil
{
    public static string MinifyString(string src)
    {
        return Regex.Replace(src, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
    }
}