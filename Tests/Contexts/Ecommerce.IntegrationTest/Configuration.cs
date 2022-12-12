namespace Ecommerce.IntegrationTest;

public static class IntegrationTestConfiguration
{
    public static readonly List<string> initScripts = new()
    {
        $"{Environment.CurrentDirectory}/SQL/Definitions:/var/lib/sql/denifitions",
        $"{Environment.CurrentDirectory}/SQL/Init.sh:/docker-entrypoint-initdb.d/init.sh"
    };
}
