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
using GotorzProject.Service;
using GotorzProject.Service.Misc;
using Stripe;

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
StripeConfiguration.ApiKey = builder.Configuration["APIKeys:Stripe:SecretKey"];

// MSSql
// PostgreSQL

string dbType = "MSSql";

var connectionString = configuration.GetConnectionString(dbType);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddDefaultIdentity<IdentityUser>()
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

// HttpClientFactory PaymentProvider
builder.Services.AddHttpClient("Stripe", client =>
{
    client.BaseAddress = new Uri("https://api.stripe.com/v1/");
    client.DefaultRequestHeaders.Add("Authorization", "Bearer" + configuration["APIKeys:Stripe:sk_test_51R93CSP59y1jtVDPRvyzpEoB06u2NV5aqGxELTalIfjrdNZbAtCrdCATkFzWI2Fp6DLxakjQRPMO0nharQX7Nh3R00H2g2cCyp"]);
});



builder.Services.AddScoped<IFlightProvider, BookingFlightProvider>();
builder.Services.AddScoped<IHotelProvider, BookingCOMHotelProvider>();
builder.Services.AddSingleton<StripeClient>(_ => new StripeClient(configuration["APIKeys:Stripe:sk_test_51R93CSP59y1jtVDPRvyzpEoB06u2NV5aqGxELTalIfjrdNZbAtCrdCATkFzWI2Fp6DLxakjQRPMO0nharQX7Nh3R00H2g2cCyp"]));
builder.Services.AddScoped<IPaymentProvider, PaymentProvider>();

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
