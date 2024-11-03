using WiredBrainCoffee.UI.Components;
using WiredBrainCoffee.Models.Services;
using WiredBrainCoffee.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IMenuService, MenuService>(client =>
    client.BaseAddress = new Uri("https://localhost:7289/"));
builder.Services.AddHttpClient<IContactService, ContactService>(client =>
    client.BaseAddress = new Uri("https://localhost:7289/"));
builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
    client.BaseAddress = new Uri("https://localhost:7289/"));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseExceptionHandler("/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
