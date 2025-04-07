using GotorzProject.Client.Pages;
using GotorzProject.Components;
using GotorzProject.Model.ObjectRelationMapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Syncfusion.Blazor;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GotorzProject.Client.Services;
using Blazored.LocalStorage;
using GotorzProject.Model;
using GotorzProject.Service;
using GotorzProject.Service.Misc;
using System.Diagnostics;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSyncfusionBlazor();

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


// todo : refactor this into a class by itself, so we can use configsection for actual real purposes
// IConfigurationSection => APIKeys
//builder.Services.AddSingleton(configuration.GetSection("APIKeys"));

// Change this if we change booking / flight api provider
builder.Services.Configure<BookingAPIModel>(builder.Configuration.GetSection("APIKeys:BookingCOM"));

// MSSql
// PostgreSQL

string dbType = "MSSql";

var connectionString = configuration.GetConnectionString(dbType);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped
);

// API Key Testing 
bool apiConfigError = false;
List<string> apiConfigErrors = new();
var missingConfigs = new[]
{
    "API:Location:Key",
    "API:Location:Host",
    "API:BookingCOM:Host",
    "API:BookingCOM:Host"
}
.Select(k => new { Key = k, Value = configuration.GetValue<string>(k) })  // Keep key-value pair
.Where(kv => string.IsNullOrEmpty(kv.Value)) // Filter missing values
.ToList();

missingConfigs.ForEach(kv =>
{
    Console.WriteLine(kv.Key);
    apiConfigError = true;
    apiConfigErrors.Add($"{kv.Key} was not set properly.");
});

if (apiConfigError)
{
    // if error occurs here, just add the missing keys in appsettings.Development.json
    // and please make sure you don't push API keys to git :)
    //throw new ConfigurationErrorsException($"Following errors occurred:\n{string.Join(Environment.NewLine, apiConfigErrors)}");
}
// API Key Testing





// Location HttpClient
builder.Services.AddHttpClient("Location", client =>
{
    client.BaseAddress = new("https://" + configuration.GetValue<string>("APIKeys:Location:Host"));
    client.DefaultRequestHeaders.Add("x-rapidapi-host", configuration.GetValue<string>("APIKeys:Location:Host"));
    client.DefaultRequestHeaders.Add("x-rapidapi-key", configuration.GetValue<string>("APIKeys:Location:Key"));
});

builder.Services.AddIdentity<CustomUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtIssuer"],
            ValidAudience = configuration["JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]))
        };
    });

// internal service setup


// HttpClientFactory skibidi
builder.Services.AddHttpClient("BookingCOM", client =>
{
    client.BaseAddress = new("https://"+configuration.GetValue<string>("APIKeys:BookingCOM:Host"));
    client.DefaultRequestHeaders.Add("x-rapidapi-host", configuration.GetValue<string>("APIKeys:BookingCOM:Host"));
    client.DefaultRequestHeaders.Add("x-rapidapi-key", configuration.GetValue<string>("APIKeys:BookingCOM:Key"));
});

builder.Services.AddScoped<IFlightProvider, BookingFlightProvider>();
builder.Services.AddScoped<IHotelProvider, BookingCOMHotelProvider>();

builder.Services.AddScoped<IUserAdminstration, UserAdminstration>();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(GotorzProject.Client._Imports).Assembly);

app.MapControllers();

app.Run();
