namespace Common;

public static class CommonModule
{
    public static void RegisterCommonModule(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.AddApiVersioning(options =>
        {
            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            options.UnsupportedApiVersionStatusCode = (int)HttpStatusCode.NotImplemented;
        });
    }
}