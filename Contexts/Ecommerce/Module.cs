namespace Ecommerce;

using Dapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Ecommerce.Application.Service;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

public static class EcommerceModule
{
    public static IServiceCollection AddEcommerceModule(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddMediatR(typeof(EcommerceModule).Assembly);

        services.AddSingleton<IDbContext, DbContext>();
        services.AddSingleton<IDbSeed, DbSeed>();
        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<IProductRepository, ProductRepository>();

        return services;
    }

    public static IHost UseEcommerceDataSeed(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        scope.ServiceProvider
            .GetRequiredService<IDbSeed>().RunAsync().Wait();

        return host;
    }
}
