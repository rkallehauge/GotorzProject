using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection.Metadata.Ecma335;
using static GotorzProject.Client.Pages.Support;
using Syncfusion.Blazor;
using GotorzProject.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using GotorzProject.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSyncfusionBlazor();

builder.Services.AddBlazorBootstrap();

// todo : fix / make this better pls
string hardcodedLocalUrl = "https://localhost:7097";

builder.Services.AddScoped(sp => new HttpClient
{
    //BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? hardcodedLocalUrl)
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) // Use real IP instead of dumb IP
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();
