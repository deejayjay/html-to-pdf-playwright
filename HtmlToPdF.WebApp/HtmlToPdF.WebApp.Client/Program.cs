using HtmlToPdF.WebApp.Client.Services;
using HtmlToPdF.WebApp.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("Reports API", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});

builder.Services.AddScoped<IReportService, ReportService>();


await builder.Build().RunAsync();
