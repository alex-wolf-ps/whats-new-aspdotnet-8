using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using WiredBrainCoffee.Client.Components;
using WiredBrainCoffee.Models.Services;
using WiredBrainCoffee.UI.Components;
using WiredBrainCoffee.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazorise()
  .AddBootstrapProviders()
  .AddFontAwesomeIcons();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IMenuService, MenuService>(client =>
    client.BaseAddress = new Uri("https://localhost:7289/"));
builder.Services.AddHttpClient<IContactService, ContactService>(client =>
    client.BaseAddress = new Uri("https://localhost:7289/"));
builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
    client.BaseAddress = new Uri("https://localhost:7289/"));

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Order).Assembly);

app.Run();
