namespace Common;

using Microsoft.AspNetCore.Routing;

public static class CommonExtensions
{
    public static IServiceCollection AddCommonConfig(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        return services;
    }
}
