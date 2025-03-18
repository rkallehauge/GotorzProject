using GotorzProject.Client.ClientAPI;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


// Define backend API URL, either from config or fallback
string backendUrl = builder.Configuration["FrontendUrl"] ?? "https://localhost:7097";

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(backendUrl) });

builder.Services.AddScoped<AuthService>();

var services = builder.Services.ToList();

Console.WriteLine("Registered services:");
foreach (var service in services)
{
    Console.WriteLine($"{service.ServiceType} {service.ServiceKey}");
}


await builder.Build().RunAsync();
