namespace Common.Fixture.Infrastructure.Database;

using Docker.DotNet;
using Docker.DotNet.Models;
using Npgsql;
using Dapper;

using System.Data.Common;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

public sealed class PostgresDatabaseFactory
{
    private int _port { get; init; }
    private string _template { get; init; }
    private string _database { get; init; }

    private const string _user = "root";
    private const string _password = "root";

    private readonly DbConnectionStringBuilder _masterConnectionString;
    private readonly DbConnectionStringBuilder _consumerConnectionString;

    public PostgresDatabaseFactory(string template, int port = 5432)
    {
        _port = port;
        _template = template;

        _database = GetRandomString(56);

        _masterConnectionString = new DbConnectionStringBuilder()
        {
            { "User ID", _user },
            { "Password", _password },
            { "Server", "localhost" },
            { "Port", _port },
            { "Database", "postgres" },
            { "Integrated Security", true },
            { "Pooling", true }
        };

        _consumerConnectionString = new DbConnectionStringBuilder()
        {
            { "User ID", _user },
            { "Password", _password },
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

        string sql = $"""
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

        string sql = $"""
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
        string allowedChars = "abcdefghijkmnopqrstuvwxyz";

        char[] chars = new char[length];
        var rd = new Random();

        for (int i = 0; i < length; i++)
        {
            chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
        }

        return new string(chars);
    }
}

// CONFIGURATION FOR CONTAINER STRATEGY:
//
//
// public static readonly List<string> PostgresDatabaseInitScripts = new()
// {
//    $"{Environment.CurrentDirectory}/SQL/Definitions:/var/lib/sql/denifitions",
//    $"{Environment.CurrentDirectory}/SQL/Init.sh:/docker-entrypoint-initdb.d/init.sh"
// };

// <ItemGroup>
//   <Content Include="..\..\..\Contexts\Ecommerce\Infrastructure\Persistence\SQL\**\*.*">
//     <Link>SQL\%(RecursiveDir)%(Filename)%(Extension)</Link>
//     <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
//   </Content>
// </ItemGroup>

public sealed class PostgresDatabase
{
    private DockerClient _docker { get; init; }
    private string _database { get; init; }
    private List<string> _initScripts { get; init; }

    private const string _image = "postgres:15-alpine";
    private readonly string _container = $"postgres-{Guid.NewGuid()}";

    public PostgresDatabase(string database, List<string> initScripts)
    {
        _database = database;
        _initScripts = initScripts;

        var dockerDaemonUri = new Uri("npipe://./pipe/docker_engine");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            dockerDaemonUri = new Uri("unix:///var/run/docker.sock");
        }

        _docker = new DockerClientConfiguration(dockerDaemonUri).CreateClient();
    }

    public async Task<string> StartAsync()
    {
        int port = GetRandomUnusedPort();

        var locale = new CultureInfo("en-US");

        const string user = "root";
        const string password = "root";

        var environment = new List<string> {
            $"POSTGRES_USER={user}",
            $"POSTGRES_PASSWORD={password}",
            $"POSTGRES_DB={_database}" };

        var connectionString = new DbConnectionStringBuilder()
        {
            { "User ID", user },
            { "Password", password },
            { "Server", "localhost" },
            { "Port", port },
            { "Database", _database },
            { "Integrated Security", true },
            { "Pooling", true }
        };

        var hostConfig = new HostConfig
        {

            Binds = _initScripts,
            PortBindings = new Dictionary<string, IList<PortBinding>>
            {
                {
                    "5432/tcp",
                    new List<PortBinding>
                    {
                        new PortBinding{
                            HostPort = port.ToString(locale)
                        }
                    }
                }
            }
        };

        await _docker.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _image,
                Name = _container,
                HostConfig = hostConfig,
                Env = environment
            }
        );

        await _docker.Containers.StartContainerAsync(_container, new ContainerStartParameters());

        await using var conn = new NpgsqlConnection(connectionString.ToString());
        while (true)
        {
            try
            {
                await conn.OpenAsync();
                break;
            }
            catch (Exception)
            {
            }
        }

        return connectionString.ToString();
    }

    public async Task DisposeAsync()
    {
        await _docker.Containers.RemoveContainerAsync(_container,
            new ContainerRemoveParameters { RemoveVolumes = true, Force = true });

        _docker.Dispose();
    }

    private static int GetRandomUnusedPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();

        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();

        return port;
    }
}
