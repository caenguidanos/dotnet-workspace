namespace Ecommerce.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class HostExtension
{
    public static void UseEcommerceSeed(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<IDbSeed>();
        service.RunAsync().Wait();
    }
}