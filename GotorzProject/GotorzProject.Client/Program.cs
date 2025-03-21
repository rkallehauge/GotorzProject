using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSyncfusionBlazor();
builder.Services.AddBlazorBootstrap();

// todo : fix / make this better pls
string hardcodedLocalUrl = "https://localhost:7097";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? hardcodedLocalUrl)
});

await builder.Build().RunAsync();
