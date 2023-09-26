using System.Text.Json.Serialization;
using WiredBrainCoffee.MinApi.Services;
using WiredBrainCoffee.MinApi.Services.Interfaces;
using WiredBrainCoffee.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRequestTimeouts();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddHttpClient();

builder.Services.AddCors();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AotJsonSerializerContext.Default);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseRequestTimeouts();

app.MapGet("/orders", (IOrderService orderService) =>
{
    Thread.Sleep(3000);
    return Results.Ok(orderService.GetOrders());
});

app.MapGet("/orders/{id}", (IOrderService orderService, int id) =>
{
    return Results.Ok(orderService.GetOrderById(id));
});

app.MapPost("/contact", (Contact contact) =>
{
    // save contact to database
});

app.MapGet("/menu", (IMenuService menuService) =>
{
    return menuService.GetMenuItems();
});

//app.Use(async (context, next) =>
//{
//    Debugger.Break();
//    await next.Invoke();
//    Debugger.Break();
//});

app.MapGet("/inventory", async (IHttpClientFactory factory, HttpContext context) =>
{
    var client = factory.CreateClient();

    await client.GetStringAsync("https://localhost:7257/", context.RequestAborted);
})
.WithRequestTimeout(TimeSpan.FromSeconds(2));

app.MapGet("/healthcheck", () => Results.Ok("Alive")).ShortCircuit();

app.MapShortCircuit(400, "robots.txt", "sitemap.xml");

app.Run();

[JsonSerializable(typeof(List<MenuItem>))]
[JsonSerializable(typeof(List<Order>))]
[JsonSerializable(typeof(Order))]
[JsonSerializable(typeof(Contact))]
public partial class AotJsonSerializerContext : JsonSerializerContext
{

}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}