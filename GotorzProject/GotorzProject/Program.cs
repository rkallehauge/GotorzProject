using GotorzProject.Client.Pages;
using GotorzProject.Components;
using GotorzProject.Model.ObjectRelationMapping;
using GotorzProject.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();


//bootstrap initialzxiton
builder.Services.AddBlazorBootstrap();

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .Build();

// MSSql
// PostgreSQL

string? curDb = (string?)configuration.GetValue(typeof(string), "CurrentUsedDB");

string? connString = configuration.GetConnectionString(curDb);


if (curDb == "MSSql")
{
    builder.Services.AddDbContext<PrimaryDbContext>(
        options => options.UseSqlServer(connString)
    );

    builder.Services.AddDbContext<ApplicationIdentityDbContext>(
        options => options.UseSqlServer(connString)
    );
}
else if (curDb == "PostgreSQL")
{
    builder.Services.AddDbContext<PrimaryDbContext>(
        options => options.UseNpgsql(connString)
    );

    builder.Services.AddDbContext<ApplicationIdentityDbContext>(
        options => options.UseNpgsql(connString)
    );
}

builder.Services.AddIdentityCore<IdentityUser>();


builder.Services.AddHttpClient();
//builder.Services.AddScoped<HttpClient>(sp =>
//    new HttpClient { BaseAddress = new Uri(builder.) });

//builder.Services.AddScoped<UserAuthenticationService>();

////builder.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();
//builder.Services.AddScoped<UserAuthenticationStateProvider>();
//builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<UserAuthenticationStateProvider>());


builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(GotorzProject.Client._Imports).Assembly);

app.MapControllers();


var services = builder.Services.ToList();

Console.WriteLine("Registered services:");
foreach (var service in services)
{
    Console.WriteLine($"{service.ServiceType} {service.ServiceKey}");
}

app.Run();