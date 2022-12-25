var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediator();

builder.Services.AddApiVersioning(options => { options.ApiVersionReader = new HeaderApiVersionReader("x-api-version"); });

builder.Services.RegisterCommonModule();
builder.Services.RegisterEcommerceModule();

var app = builder.Build();

app.MapEcommerceEndpoints();

app.Run();

public partial class Program
{
}