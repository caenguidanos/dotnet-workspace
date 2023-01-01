namespace Ecommerce;

using Ecommerce.Application.Service;
using Ecommerce.Domain.Repository;
using Ecommerce.Domain.Service;
using Ecommerce.Infrastructure.HttpHandler;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repository;

public static class EcommerceModule
{
    public static void RegisterEcommerceModule(this IServiceCollection services)
    {
        services.AddSingleton<IDbContext, DbContext>();
        services.AddTransient<IDbSeed, DbSeed>();

        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<IProductCreatorService, ProductCreatorService>();
        services.AddSingleton<IProductUpdaterService, ProductUpdaterService>();
        services.AddSingleton<IProductRemoverService, ProductRemoverService>();

        services.AddSingleton<GetProductsHttpHandler>();
        services.AddSingleton<GetProductByIdHttpHandler>();
        services.AddSingleton<CreateProductHttpHandler>();
        services.AddSingleton<RemoveProductByIdHttpHandler>();
        services.AddSingleton<UpdateProductHttpHandler>();
    }

    public static void MapEcommerceEndpoints(this WebApplication app)
    {
        var router = app
            .NewVersionedApi()
            .HasApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0));

        var getProductsHttpHandler = router.ServiceProvider.GetRequiredService<GetProductsHttpHandler>();
        var getProductByIdHttpHandler = router.ServiceProvider.GetRequiredService<GetProductByIdHttpHandler>();
        var createProductHttpHandler = router.ServiceProvider.GetRequiredService<CreateProductHttpHandler>();
        var removeProductByIdHttpHandler = router.ServiceProvider.GetRequiredService<RemoveProductByIdHttpHandler>();
        var updateProductHttpHandler = router.ServiceProvider.GetRequiredService<UpdateProductHttpHandler>();

        router.MapGet("/product", getProductsHttpHandler.HandleAsync);
        router.MapGet("/product/{id:guid}", getProductByIdHttpHandler.HandleAsync);
        router.MapPost("/product", createProductHttpHandler.HandleAsync);
        router.MapDelete("/product/{id:guid}", removeProductByIdHttpHandler.HandleAsync);
        router.MapPut("/product/{id:guid}", updateProductHttpHandler.HandleAsync);
    }

    public static void UseEcommerceSeed(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IDbSeed>();
        service.PopulateAsync().Wait();
    }
}