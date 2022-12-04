using api.Contexts.Ecommerce.Store;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddEcommerceStoreConfig(builder.Configuration);
builder.Services.AddEcommerceStoreDependencies();

var app = builder.Build();

builder.Configuration.AddEnvironmentVariables(prefix: "Api_");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();

