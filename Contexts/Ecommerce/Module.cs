namespace Contexts.Ecommerce;

using Contexts.Ecommerce.Application.Service;
using Contexts.Ecommerce.Domain.Repository;
using Contexts.Ecommerce.Domain.Service;
using Contexts.Ecommerce.Infrastructure.Persistence;
using Contexts.Ecommerce.Infrastructure.Repository;

public static class Module
{
    public static IServiceCollection AddEcommerceStoreModule(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddMediatR(typeof(Module).Assembly);

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
