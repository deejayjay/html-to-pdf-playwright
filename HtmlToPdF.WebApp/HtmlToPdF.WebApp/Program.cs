using HtmlToPdF.WebApp.Client.Services;
using HtmlToPdF.WebApp.Client.Services.Interfaces;
using HtmlToPdF.WebApp.Components;
using HtmlToPdF.WebApp.Services;
using HtmlToPdF.WebApp.Services.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddSingleton<IPdfBrowserService, PdfBrowserService>();


builder.Services.AddScoped<IHtmlGeneratorService, HtmlGeneratorService>();

builder.Services.AddHttpClient("Reports API", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});

builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HtmlToPdF.WebApp.Client._Imports).Assembly);

app.Run();