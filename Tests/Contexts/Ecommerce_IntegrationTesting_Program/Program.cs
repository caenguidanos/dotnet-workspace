var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    serverOptions.Limits.MaxConcurrentConnections = 100;
    serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
    serverOptions.Limits.MaxRequestBodySize = 1 * 1024;

    serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(
        bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));

    serverOptions.Limits.MinResponseDataRate = new MinDataRate(
        bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));

    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(1);
});

builder.Services.AddProblemDetails();
builder.Services.AddMediator();

builder.Services.RegisterCommonModule();
builder.Services.RegisterEcommerceModule();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapEcommerceEndpoints();

app.Run();

public partial class Program
{
}