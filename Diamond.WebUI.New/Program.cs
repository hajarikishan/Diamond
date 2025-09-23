using Diamond.WebUI.New.Components;
using Diamond.WebUI.New.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers(); // optional if you host controllers

// HttpClient that calls your running API
builder.Services.AddHttpClient("DiamondAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5218/"); // adjust port for Diamond.API
});
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DiamondAPI"));

// Authorization for Blazor components (no middleware JWT challenge)
builder.Services.AddAuthorizationCore();

// Register UI services
builder.Services.AddScoped<AuthService>(); // UI-side service calling API
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddAntiforgery();

builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see http://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
