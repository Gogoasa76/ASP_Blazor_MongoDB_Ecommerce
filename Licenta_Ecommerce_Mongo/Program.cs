using Licenta_Ecommerce_Mongo.Authentication;
using Licenta_Ecommerce_Mongo.DBConnections;
using Licenta_Ecommerce_Mongo.Utilitary;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddSingleton<UserAccountService>();
builder.Services.AddSingleton<MongoDBWrapper>();

builder.Services.AddMudServices();
var app = builder.Build();

UserUtilitary.CreateDefaultAdminIfNoneExist();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
