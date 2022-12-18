namespace Ecommerce.IntegrationTest.Util;

using System.Text.RegularExpressions;

public static class JsonUtil
{
    public static string CleanString(string src)
    {
        return Regex.Replace(src, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
    }
}