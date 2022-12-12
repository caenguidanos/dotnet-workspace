using Microsoft.AspNetCore.ResponseCompression;

using System.IO.Compression;

using Common;
using Common.Application.Exceptions;

using Ecommerce;
using Ecommerce.Application.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

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

app.UseResponseCompression();

app.MapControllers();
app.MapHealthChecks("/Healthz");
app.UseCors();

app.Run();
