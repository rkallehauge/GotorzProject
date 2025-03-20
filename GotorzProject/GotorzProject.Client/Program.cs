using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection.Metadata.Ecma335;
using static GotorzProject.Client.Pages.Support;
using Syncfusion.Blazor;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSyncfusionBlazor();

// todo : fix / make this better pls
string hardcodedLocalUrl = "https://localhost:7097";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? hardcodedLocalUrl)
});

await builder.Build().RunAsync();
