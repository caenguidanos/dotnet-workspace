using Common.Extensions;
using Ecommerce.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#if DEBUG
builder.Services.AddSwaggerGen();
#endif

builder.Services.AddCommonConfig();

builder.Services.AddEcommerceServices();

#if DEBUG
builder.Services.AddEcommerceSeed();
#endif

var app = builder.Build();

#if DEBUG
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseEcommerceSeed();
}
#endif

app.MapControllers();

app.UseCors();

app.Run();

public partial class Program { }