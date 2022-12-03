using MediatR;
using api.Contexts.Ecommerce.Store.Application.Service;
using api.Contexts.Ecommerce.Store.Infrastructure.Repository;
using api.Contexts.Ecommerce.Store.Domain.Service;
using api.Contexts.Ecommerce.Store.Domain.Repository;

namespace api.Contexts.Ecommerce.Store
{
    public static class EcommerceStoreModule
    {
        public static IServiceCollection AddEcommerceStoreModule(this IServiceCollection services)
        {
            services.AddMediatR(typeof(EcommerceStoreModule).Assembly);

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}