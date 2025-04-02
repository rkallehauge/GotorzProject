using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


// todo : fix / make this better pls
string hardcodedLocalUrl = "https://localhost:7097";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? hardcodedLocalUrl)
});

var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var stripeSettings = await httpClient.GetFromJsonAsync<StripeSettings>("appsettings.json");

if (stripeSettings != null)
{
    builder.Services.AddSingleton(stripeSettings);
}
await builder.Build().RunAsync();

    public class StripeSettings                     
    {
    public string SecretKey { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
    }
