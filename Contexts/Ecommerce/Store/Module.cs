namespace Ecommerce.Store;

using Ecommerce.Store.Application.Service;
using Ecommerce.Store.Domain.Repository;
using Ecommerce.Store.Domain.Service;
using Ecommerce.Store.Infrastructure.Environment;
using Ecommerce.Store.Infrastructure.Repository;

public static class EcommerceStoreModule
{
    public static IServiceCollection AddEcommerceStoreContext(this IServiceCollection services)
    {
        services
            .AddMediatR(typeof(EcommerceStoreModule).Assembly)
            .AddSingleton<ConfigurationSettings>()
            .AddSingleton<IProductService, ProductService>()
            .AddSingleton<IProductRepository, ProductRepository>();

        return services;
    }
}
