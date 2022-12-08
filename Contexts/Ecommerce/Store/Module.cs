namespace Ecommerce.Store;

using Ecommerce.Store.Application.Service;
using Ecommerce.Store.Domain.Repository;
using Ecommerce.Store.Domain.Service;
using Ecommerce.Store.Infrastructure.Persistence;
using Ecommerce.Store.Infrastructure.Repository;
using Microsoft.Extensions.Hosting;

public static class EcommerceStoreModule
{
    public static IServiceCollection AddEcommerceStoreModule(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddMediatR(typeof(EcommerceStoreModule).Assembly);

        services.AddSingleton<DbContext>();
        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<IProductRepository, ProductRepository>();

        return services;
    }

    public static IHost UseEcommerceStoreSeed(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        new DbSeed(
            scope.ServiceProvider.GetRequiredService<DbContext>()).Run();

        return host;
    }
}
