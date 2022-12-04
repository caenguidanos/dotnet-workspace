using MediatR;
using api.Contexts.Ecommerce.Store.Application.Service;
using api.Contexts.Ecommerce.Store.Infrastructure.Repository;
using api.Contexts.Ecommerce.Store.Domain.Service;
using api.Contexts.Ecommerce.Store.Domain.Repository;
using api.Contexts.Ecommerce.Store.Infrastructure.Persistence;

namespace api.Contexts.Ecommerce.Store
{
    public static class EcommerceStoreModule
    {
        public static IServiceCollection AddEcommerceStoreConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("Database"));

            return services;
        }

        public static IServiceCollection AddEcommerceStoreDependencies(this IServiceCollection services)
        {
            services.AddMediatR(typeof(EcommerceStoreModule).Assembly);

            services.AddSingleton<DatabaseClient>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IProductRepository, ProductRepository>();

            return services;
        }
    }
}