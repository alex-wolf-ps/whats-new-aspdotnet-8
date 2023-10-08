using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using WiredBrainCoffee.MinApi;
using WiredBrainCoffee.MinApi.Services;
using WiredBrainCoffee.MinApi.Services.Interfaces;
using WiredBrainCoffee.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRequestTimeouts();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddKeyedScoped<IOrderService, OrderService>("orders");
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddHttpClient();
builder.Services.AddRazorComponents();

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

app.MapGet("/orders", ([FromKeyedServices("orders")]IOrderService orderService) =>
{
    return orderService.GetOrders();
});

app.MapGet("/orders/{id}", ([FromKeyedServices("orders")]IOrderService orderService, int id) =>
{
    return orderService.GetOrderById(id);
});

app.MapGet("/antiforgery", (HttpContext context, IAntiforgery antiforgery) =>
{
    return antiforgery.GetAndStoreTokens(context);
});

app.MapPost("/contact", (HttpContext context, [FromForm]Contact contact) =>
{
    contact.SubmittedTime = DateTime.Now;

    return contact;
});

//app.MapPost("/contact-collection", (IFormCollection collection) =>
//{
//    var name = collection["name"];

//    // TODO: Save to db
//});

app.MapGet("/menu", (IMenuService menuService) =>
{
    return menuService.GetMenuItems();
});

app.MapGet("/email", (IMenuService menuService) =>
{
    return new RazorComponentResult<EmailConfirm>();
});

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