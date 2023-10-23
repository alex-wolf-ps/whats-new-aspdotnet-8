using System.Diagnostics;
using System.Text.Json.Serialization;
using WiredBrainCoffee.MinApi.Services;
using WiredBrainCoffee.MinApi.Services.Interfaces;
using WiredBrainCoffee.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddOutputCache();
builder.Services.AddStackExchangeRedisOutputCache(options =>
{
    options.Configuration = "your_connection_string";
    options.InstanceName = "dotnet8redis";
});
builder.Services.AddRequestTimeouts();
builder.Services.AddHttpClient();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain
    .Insert(0, AotJsonSerializerContext.Default);
});

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOutputCache();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseRequestTimeouts();

app.Use(async (context, next) =>
{
    // Executes at start of middleware pipeline for incoming request
    Debugger.Break();

    await next.Invoke();

    // Executes at end of middleware pipeline for outgoing response
    Debugger.Break();
});

app.MapGet("/liveness", () =>
{
    return "Alive";
}).ShortCircuit();

app.MapShortCircuit(400, "robots.txt", "sitemap.xml");

app.MapGet("/orders", (IOrderService orderService) =>
{
    return orderService.GetOrders();
}).CacheOutput();

app.MapGet("/orders/{id}", (IOrderService orderService, int id) =>
{
    return orderService.GetOrderById(id);
});

app.MapPost("/contact", (Contact contact) =>
{
    contact.SubmittedTime = DateTime.Now;

    return contact;
});

app.MapGet("/menu", (IMenuService menuService) =>
{
    return menuService.GetMenuItems();
})
.CacheOutput();

app.MapGet("/inventory", async (HttpContext context, IHttpClientFactory factory) => {

    var client = factory.CreateClient();
    var stockCount = await client
    .GetStringAsync("https://localhost:7210/inventory", context.RequestAborted);

    return stockCount;
})
.WithRequestTimeout(TimeSpan.FromSeconds(5));

app.Run();

[JsonSerializable(typeof(List<MenuItem>))]
[JsonSerializable(typeof(List<Order>))]
[JsonSerializable(typeof(Order))]
[JsonSerializable(typeof(Contact))]
public partial class AotJsonSerializerContext : JsonSerializerContext
{

}
