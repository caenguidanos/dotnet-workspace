using Common;
using Ecommerce;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediator();
builder.Services.AddControllers();

builder.Services.AddCommonConfig();
builder.Services.AddEcommerceServices();

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { }