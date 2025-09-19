using Diamond.WebUI.Components;
using Diamond.WebUI.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();

// Auth services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Blazor auth primitives (for <AuthorizeView> + [Authorize] in Razor components)
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthenticationCore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
