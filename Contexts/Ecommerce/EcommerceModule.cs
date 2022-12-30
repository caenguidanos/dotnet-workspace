namespace Ecommerce;

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

        router.MapGet("/product", router.ServiceProvider.GetRequiredService<GetProductsHttpHandler>().HandleAsync);
        router.MapGet("/product/{id:guid}", router.ServiceProvider.GetRequiredService<GetProductByIdHttpHandler>().HandleAsync);
        router.MapPost("/product", router.ServiceProvider.GetRequiredService<CreateProductHttpHandler>().HandleAsync);
        router.MapDelete("/product/{id:guid}", router.ServiceProvider.GetRequiredService<RemoveProductByIdHttpHandler>().HandleAsync);
        router.MapPut("/product/{id:guid}", router.ServiceProvider.GetRequiredService<UpdateProductHttpHandler>().HandleAsync);
    }

    public static void UseEcommerceSeed(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IDbSeed>();
        service.PopulateAsync().Wait();
    }
}