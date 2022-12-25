using Common;

using Ecommerce;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddCors();
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

app.UseStatusCodePages();

app.MapEcommerceEndpoints();

app.UseCors();

app.Run();