namespace Ecommerce_IntegrationTesting_Program;

public sealed class PostgresFactory
{
    private int _port { get; }
    private string _template { get; }
    private string _database { get; }

    private const string User = "root";
    private const string Password = "root";

    private readonly DbConnectionStringBuilder _masterConnectionString;
    private readonly DbConnectionStringBuilder _consumerConnectionString;

    public PostgresFactory(string template, int port = 5432)
    {
        _port = port;
        _template = template;
        _database = GetRandomString(56);

        _masterConnectionString = new DbConnectionStringBuilder
        {
            { "User ID", User },
            { "Password", Password },
            { "Server", "localhost" },
            { "Port", _port },
            { "Database", "postgres" },
            { "Integrated Security", true },
            { "Pooling", true }
        };

        _consumerConnectionString = new DbConnectionStringBuilder
        {
            { "User ID", User },
            { "Password", Password },
            { "Server", "localhost" },
            { "Port", _port },
            { "Database", _database },
            { "Integrated Security", true },
            { "Pooling", true }
        };
    }

    public async Task<string> StartAsync()
    {
        await using var conn = new NpgsqlConnection(_masterConnectionString.ToString());
        await conn.OpenAsync();

        var sql = $"""
            SELECT pg_terminate_backend(pid) 
            FROM pg_stat_activity 
            WHERE pid <> pg_backend_pid()
            AND datname = '{_template}';

            CREATE DATABASE {_database} TEMPLATE {_template};
        """;

        await conn.ExecuteAsync(sql);

        return _consumerConnectionString.ToString();
    }

    public async Task DisposeAsync()
    {
        await using var conn = new NpgsqlConnection(_masterConnectionString.ToString());
        await conn.OpenAsync();

        var sql = $"""
            SELECT pg_terminate_backend(pid) 
            FROM pg_stat_activity 
            WHERE pid <> pg_backend_pid()
            AND datname = '{_database}';

            DROP DATABASE {_database};
        """;

        await conn.ExecuteAsync(sql);
    }

    private static string GetRandomString(int length)
    {
        const string allowedChars = "abcdefghijkmnopqrstuvwxyz";

        var chars = new char[length];
        var rd = new Random();

        for (var i = 0; i < length; i++)
        {
            chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
        }

        return new string(chars);
    }
}