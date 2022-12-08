namespace Ecommerce.IntegrationTest.Fixture;

using System.Data.Common;
using System.Runtime.InteropServices;
using Docker.DotNet;
using Docker.DotNet.Models;

public class PostgresFixture
{
    private DockerClient _docker;

    public string PostgresImageName { get; } = "postgres:15-alpine";
    public string PostgresContainerName { get; } = $"postgres-{Guid.NewGuid()}";

    public PostgresFixture()
    {
        Uri dockerDaemonUri;

        var isPlatformOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        var isPlatformLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        var isPlatformWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        if (isPlatformWindows)
        {
            dockerDaemonUri = new Uri("npipe://./pipe/docker_engine");
        }
        else if (isPlatformLinux)
        {
            dockerDaemonUri = new Uri("unix:///var/run/docker.sock");
        }
        else if (isPlatformOSX)
        {
            dockerDaemonUri = new Uri("unix:///var/run/docker.sock");
        }
        else
        {
            throw new NotSupportedException($"Unsupported OS [{RuntimeInformation.OSDescription}]");
        }

        _docker = new DockerClientConfiguration(dockerDaemonUri).CreateClient();
    }

    public string Start(int port, string database)
    {
        var task = StartAsync(port, database);
        task.Wait();
        return task.Result;
    }

    public async Task<string> StartAsync(int port, string database)
    {
        var environment = new List<string>();
        environment.Add("POSTGRES_USER=root");
        environment.Add("POSTGRES_PASSWORD=root");
        environment.Add($"POSTGRES_DB={database}");

        var connectionString = new DbConnectionStringBuilder();
        connectionString.Add("User ID", "root");
        connectionString.Add("Password", "root");
        connectionString.Add("Server", "localhost");
        connectionString.Add("Port", port);
        connectionString.Add("Database", database);
        connectionString.Add("Integrated Security", true);
        connectionString.Add("Pooling", true);

        var hostConfig = new HostConfig
        {
            PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {
                            "5432/tcp",
                            new List<PortBinding>
                            {
                                new PortBinding{
                                    HostPort = port.ToString()
                                }
                            }
                        }
                    }
        };

        var container = await _docker.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = PostgresImageName,
                Name = PostgresContainerName,
                HostConfig = hostConfig,
                Env = environment
            }
        );

        await _docker.Containers.StartContainerAsync(
            PostgresContainerName, new ContainerStartParameters());

        return connectionString.ToString();
    }

    public void Dispose()
    {
        var task = DisposeAsync();
        task.Wait();
    }

    public async Task DisposeAsync()
    {
        await _docker.Containers.RemoveContainerAsync(
            PostgresContainerName, new ContainerRemoveParameters { Force = true });

        _docker.Dispose();
    }
}