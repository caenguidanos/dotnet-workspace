using Common;
using Common.Application.Exceptions;

using Ecommerce;
using Ecommerce.Application.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<EcommerceExceptionFilter>();
    options.Filters.Add<FallbackExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddCommonModule();
builder.Services.AddEcommerceModule();

var app = builder.Build();

#if DEBUG
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseEcommerceDataSeed();
}
#endif

app.MapControllers();

app.MapHealthChecks("/Healthz");

app.UseCors();

app.Run();
