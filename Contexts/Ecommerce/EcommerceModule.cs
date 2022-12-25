namespace Ecommerce;

public static class EcommerceModule
{
    public static void RegisterEcommerceModule(this IServiceCollection services)
    {
        services.AddSingleton<IDbContext, DbContext>();

        services.AddTransient<IDbSeed, DbSeed>();

        services.AddSingleton<IProductCreatorService, ProductCreatorService>();
        services.AddSingleton<IProductUpdaterService, ProductUpdaterService>();
        services.AddSingleton<IProductRemoverService, ProductRemoverService>();
        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<IGetProductsEndpoint, GetProductsEndpoint>();
        services.AddSingleton<IGetProductByIdEndpoint, GetProductByIdEndpoint>();
        services.AddSingleton<ICreateProductEndpoint, CreateProductEndpoint>();
        services.AddSingleton<IDeleteProductEndpoint, DeleteProductEndpoint>();
        services.AddSingleton<IUpdateProductEndpoint, UpdateProductEndpoint>();
    }

    public static void MapEcommerceEndpoints(this WebApplication app)
    {
        var router = app
            .NewVersionedApi()
            .HasApiVersion(new ApiVersion(majorVersion: 1, minorVersion: 0));

        router.MapGet("/product", router.ServiceProvider.GetRequiredService<IGetProductsEndpoint>().HandleAsync);
        router.MapGet("/product/{id:guid}", router.ServiceProvider.GetRequiredService<IGetProductByIdEndpoint>().HandleAsync);
        router.MapPost("/product", router.ServiceProvider.GetRequiredService<ICreateProductEndpoint>().HandleAsync);
        router.MapDelete("/product/{id:guid}", router.ServiceProvider.GetRequiredService<IDeleteProductEndpoint>().HandleAsync);
        router.MapPut("/product/{id:guid}", router.ServiceProvider.GetRequiredService<IUpdateProductEndpoint>().HandleAsync);
    }

    public static void UseEcommerceSeed(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IDbSeed>();
        service.PopulateAsync().Wait();
    }
}