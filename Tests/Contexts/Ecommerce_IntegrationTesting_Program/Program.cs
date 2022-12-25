var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddMediator();

builder.Services.RegisterCommonModule();
builder.Services.RegisterEcommerceModule();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapEcommerceEndpoints();

app.Run();

public partial class Program
{
}