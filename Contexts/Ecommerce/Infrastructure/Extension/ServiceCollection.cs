namespace Ecommerce.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

using Application;

using Domain;

public static class ServiceCollectionExtension
{
    public static void AddEcommerceServices(this IServiceCollection services)
    {
        services.AddSingleton<IDbContext, DbContext>();
        services.AddSingleton<IProductCreatorService, ProductCreatorService>();
        services.AddSingleton<IProductUpdaterService, ProductUpdaterService>();
        services.AddSingleton<IProductRemoverService, ProductRemoverService>();
        services.AddSingleton<IProductRepository, ProductRepository>();
    }

    public static void AddEcommerceSeed(this IServiceCollection services)
    {
        services.AddScoped<IDbSeed, DbSeed>();
    }
}