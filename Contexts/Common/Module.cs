namespace Common;

using Common.Application.Service;
using Common.Domain.Service;

public static class CommonModule
{
    public static IServiceCollection AddCommonContext(this IServiceCollection services)
    {
        services.AddScoped<ILoggerService, LoggerService>();

        return services;
    }
}
