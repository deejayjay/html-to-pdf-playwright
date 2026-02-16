using HtmlToPdF.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HtmlToPdF.WebApp.Services;

public class HtmlGeneratorService(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : IHtmlGeneratorService
{
    public async Task<string> GenerateAsync<TComponent, TModel>(TModel model, Dictionary<string, object?>? parameters = null)
        where TComponent : IComponent
    {
        using var renderer = new HtmlRenderer(serviceProvider, loggerFactory);

        var html = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var parameterView = parameters != null
                ? ParameterView.FromDictionary(parameters)
                : ParameterView.Empty;

            var result = await renderer.RenderComponentAsync<TComponent>(parameterView);

            return result.ToHtmlString();
        });

        return html;
    }
}