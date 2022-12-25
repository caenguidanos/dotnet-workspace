using Asp.Versioning;

using Common;

using Ecommerce;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddCors();

builder.Services.AddMediator();

builder.Services.AddApiVersioning(options => { options.ApiVersionReader = new HeaderApiVersionReader("x-api-version"); });

builder.Services.RegisterCommonModule();
builder.Services.RegisterEcommerceModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseEcommerceSeed();
}

app.MapEcommerceEndpoints();

app.UseCors();

app.Run();