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
    .AddJsonFile("appsettings.json") // does this even work ???
    //.AddJsonFile("appsettings.Development.json")
    .AddEnvironmentVariables() // Cloud cant be fucked to use hardcoded files anymore, very sad :(
    .Build();

Console.WriteLine("All config KVPS");
Console.WriteLine("All config KVPS");
Console.WriteLine("All config KVPS");

foreach (var kvp in configuration.AsEnumerable())
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    //Console.WriteLine(configuration.GetValue<string>(kvp.Key));
}

Console.WriteLine("All config KVPS");
Console.WriteLine("All config KVPS");
Console.WriteLine("All config KVPS");
//// todo : refactor this into a class by itself, so we can use configsection for actual real purposes
// IConfigurationSection => APIKeys
//builder.Services.AddSingleton(configuration.GetSection("APIKeys"));

// MSSql
// PostgreSQL

string dbType = "PostgreSQL";
//string dbType = "MSSql";

var connectionString = configuration.GetConnectionString(dbType);

Console.WriteLine(connectionString);

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString), ServiceLifetime.Scoped
//);
// We are currently using PostgreSQL on server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Scoped
);



// API Key Testing 
bool apiConfigError = false;
List<string> apiConfigErrors = new();
var missingConfigs = new[]
{
    "APIKeys:Location:Key",
    "APIKeys:Location:Host",
    "APIKeys:BookingCOM:Host",
    "APIKeys:BookingCOM:Host"
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






builder.Services.AddIdentity<CustomUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // DOES THIS WORK
        // NOONE FUCKING KNOWS LETS FUCKING GOOOO
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


// BookingCOM HttpClient
builder.Services.AddHttpClient("BookingCOM", client =>
{
    client.BaseAddress = new("https://"+configuration.GetValue<string>("APIKeys:BookingCOM:Host"));
    client.DefaultRequestHeaders.Add("x-rapidapi-host", configuration.GetValue<string>("APIKeys:BookingCOM:Host"));
    client.DefaultRequestHeaders.Add("x-rapidapi-key", configuration.GetValue<string>("APIKeys:BookingCOM:Key"));
});


// Location HttpClient
builder.Services.AddHttpClient("Location", client =>
{
    client.BaseAddress = new("https://" + configuration.GetValue<string>("APIKeys:Location:Host"));
    client.DefaultRequestHeaders.Add("x-rapidapi-host", configuration.GetValue<string>("APIKeys:Location:Host"));
    client.DefaultRequestHeaders.Add("x-rapidapi-key", configuration.GetValue<string>("APIKeys:Location:Key"));
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



// Manage migrations on before running
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    // Check whether database setup is relational
    if (dbContext.Database.IsRelational())
    {
        Console.WriteLine("Database is relational.");
        try
        {
            // If so, run a migration
            // This will only be resource intensive if there actually are changes
            dbContext.Database.Migrate();
        } catch (InvalidOperationException e)
        {
            Console.WriteLine("No migrations to add.");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Data);
            Console.WriteLine(e.Source);
            Console.WriteLine(e.StackTrace);
        }
    }
    else
    {
        Console.WriteLine("Database is NOT relational...?");
    }
}


app.Run();
