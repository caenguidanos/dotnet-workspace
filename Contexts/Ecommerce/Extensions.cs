namespace Ecommerce.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Ecommerce.Application.Service;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

public static class EcommerceExtensions
{
    public static IServiceCollection AddEcommerceServices(this IServiceCollection services)
    {
        services.AddMediator();

        services.AddSingleton<IDbContext, DbContext>();
        services.AddSingleton<IProductCreatorService, ProductCreatorService>();
        services.AddSingleton<IProductUpdaterService, ProductUpdaterService>();
        services.AddSingleton<IProductRemoverService, ProductRemoverService>();
        services.AddSingleton<IProductRepository, ProductRepository>();

        return services;
    }

    public static IServiceCollection AddEcommerceSeed(this IServiceCollection services)
    {
        services.AddSingleton<IDbSeed, DbSeed>();

        return services;
    }

    public static IHost UseEcommerceSeed(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        scope.ServiceProvider.GetRequiredService<IDbSeed>().RunAsync().Wait();

        return host;
    }
}

