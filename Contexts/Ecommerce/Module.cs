namespace Ecommerce;

using Ecommerce.Application.Service;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

public static class Module
{
    public static IServiceCollection AddEcommerceModule(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddMediatR(typeof(Module).Assembly);

        services.AddSingleton<IDbContext, DbContext>();
        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<IProductRepository, ProductRepository>();

        return services;
    }

    public static IHost UseEcommerceDataSeed(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        new DbSeed(scope.ServiceProvider.GetRequiredService<DbContext>())
            .RunAsync()
            .Wait();

        return host;
    }
}
