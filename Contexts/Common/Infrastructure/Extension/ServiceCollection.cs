namespace Common.Infrastructure;

public static class ServiceCollectionExtension
{
    public static void AddCommonConfig(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });
    }
}