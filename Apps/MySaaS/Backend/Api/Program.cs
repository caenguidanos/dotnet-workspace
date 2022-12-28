using Microsoft.AspNetCore.Server.Kestrel.Core;

using Common;

using Ecommerce;

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

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithHeaders("x-api-version")
            .WithMethods("GET", "OPTIONS")
            .AllowAnyOrigin();
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddMediator();

builder.Services.RegisterCommonModule();
builder.Services.RegisterEcommerceModule();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseEcommerceSeed();
}

app.UseCors();

app.UseStatusCodePages();

app.MapEcommerceEndpoints();

app.Run();