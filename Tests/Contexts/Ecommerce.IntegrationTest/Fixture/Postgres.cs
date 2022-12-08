namespace Ecommerce.IntegrationTest.Fixture;

using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;
using Docker.DotNet;
using Docker.DotNet.Models;

public class PostgresFixture
{
    private readonly DockerClient _docker;

    private readonly string _postgresImageName = "postgres:15-alpine";
    private readonly string _postgresContainerName = $"postgres-{Guid.NewGuid()}";

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

    public string StartServer(int port, string database)
    {
        var task = StartServerAsync(port, database);
        task.Wait();
        return task.Result;
    }

    public async Task<string> StartServerAsync(int port, string database)
    {
        var environment = new List<string>
        {
            "POSTGRES_USER=root",
            "POSTGRES_PASSWORD=root",
            $"POSTGRES_DB={database}"
        };

        var connectionString = new DbConnectionStringBuilder()
        {
            { "User ID", "root" },
            { "Password", "root" },
            { "Server", "localhost" },
            { "Port", port },
            { "Database", database },
            { "Integrated Security", true },
            { "Pooling", true }
        };

        var locale = new CultureInfo("en-US");

        var hostConfig = new HostConfig
        {
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

        await _docker.Containers.StartContainerAsync(
            _postgresContainerName, new ContainerStartParameters());

        return connectionString.ToString();
    }

    public void DisposeServer()
    {
        var task = DisposeServerAsync();
        task.Wait();
    }

    public async Task DisposeServerAsync()
    {
        await _docker.Containers.RemoveContainerAsync(
            _postgresContainerName, new ContainerRemoveParameters { Force = true });

        _docker.Dispose();
    }
}
