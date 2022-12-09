namespace Ecommerce.IntegrationTest.Fixture;

using Docker.DotNet;
using Docker.DotNet.Models;
using Npgsql;
using System.Data.Common;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

public class PostgresFixture
{
    private readonly DockerClient _docker;

    private readonly string _postgresDatabase = "ecommerce";
    private readonly string _postgresImageName = "postgres:15-alpine";
    private readonly string _postgresContainerName = $"postgres-{Guid.NewGuid()}";

    private readonly List<string> _dockerContainerVolumes = new()
    {
        $"{Environment.CurrentDirectory}/SQL/Definitions:/var/lib/sql/denifitions",
        $"{Environment.CurrentDirectory}/SQL/Init.sh:/docker-entrypoint-initdb.d/init.sh"
    };

    public PostgresFixture()
    {
        var dockerDaemonUri = new Uri("npipe://./pipe/docker_engine");

        bool isPlatformOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        bool isPlatformLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        if (isPlatformLinux || isPlatformOSX)
        {
            dockerDaemonUri = new Uri("unix:///var/run/docker.sock");
        }

        _docker = new DockerClientConfiguration(dockerDaemonUri).CreateClient();
    }

    public string StartServer()
    {
        var task = StartServerAsync();
        task.Wait();
        return task.Result;
    }

    public async Task<string> StartServerAsync()
    {
        int port = GetRandomUnusedPort();

        var environment = new List<string>
        {
            "POSTGRES_USER=root",
            "POSTGRES_PASSWORD=root",
            $"POSTGRES_DB={_postgresDatabase}"
        };

        var connectionString = new DbConnectionStringBuilder()
        {
            { "User ID", "root" },
            { "Password", "root" },
            { "Server", "localhost" },
            { "Port", port },
            { "Database", _postgresDatabase },
            { "Integrated Security", true },
            { "Pooling", true }
        };

        var locale = new CultureInfo("en-US");

        var hostConfig = new HostConfig
        {

            Binds = _dockerContainerVolumes,
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
                Image = _postgresImageName,
                Name = _postgresContainerName,
                HostConfig = hostConfig,
                Env = environment
            }
        );

        await _docker.Containers.StartContainerAsync(_postgresContainerName, new ContainerStartParameters());

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

    public void DisposeServer()
    {
        var task = DisposeServerAsync();
        task.Wait();
    }

    public async Task DisposeServerAsync()
    {
        await _docker.Containers.RemoveContainerAsync(_postgresContainerName, new ContainerRemoveParameters { Force = true });

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
