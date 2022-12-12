using Common;
using Ecommerce;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllers();
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
