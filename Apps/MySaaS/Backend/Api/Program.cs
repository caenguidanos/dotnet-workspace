using Common;
using Ecommerce;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#if DEBUG
builder.Services.AddSwaggerGen();
#endif

builder.Services.AddCommonServices();
builder.Services.AddEcommerceServices();

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
app.UseCors();
app.Run();
