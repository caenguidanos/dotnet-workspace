using MediatR;
using api.Contexts.Ecommerce.Store.Application.Service;
using api.Contexts.Ecommerce.Store.Infrastructure.Repository;
using api.Contexts.Ecommerce.Store.Domain.Service;
using api.Contexts.Ecommerce.Store.Domain.Repository;
using api.Contexts.Ecommerce.Store.Infrastructure.Environment;

namespace api.Contexts.Ecommerce.Store
{
    public static class EcommerceStoreModule
    {
        public static IServiceCollection AddEcommerceStoreModule(this IServiceCollection services)
        {
            services.AddMediatR(typeof(EcommerceStoreModule).Assembly);

            services.AddSingleton<ConfigurationSettings>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IProductRepository, ProductRepository>();

            return services;
        }
    }
}