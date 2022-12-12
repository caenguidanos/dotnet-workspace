namespace Ecommerce.AcceptanceTest;

public static class AcceptanceTestConfiguration
{
    public static readonly List<string> initScripts = new()
    {
        $"{Environment.CurrentDirectory}/SQL/Definitions:/var/lib/sql/denifitions",
        $"{Environment.CurrentDirectory}/SQL/Init.sh:/docker-entrypoint-initdb.d/init.sh"
    };
}
