namespace Ecommerce.IntegrationTest;

public class Config
{
    public static readonly List<string> postgresDatabaseVolumes = new(){
        $"{Environment.CurrentDirectory}/SQL/Definitions:/var/lib/sql/denifitions",
        $"{Environment.CurrentDirectory}/SQL/Init.sh:/docker-entrypoint-initdb.d/init.sh"
    };
}
