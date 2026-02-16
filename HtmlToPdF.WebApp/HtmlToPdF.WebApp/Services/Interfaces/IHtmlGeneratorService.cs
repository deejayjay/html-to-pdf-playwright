using Microsoft.AspNetCore.Components;

namespace HtmlToPdF.WebApp.Services.Interfaces
{
    public interface IHtmlGeneratorService
    {
        Task<string> GenerateAsync<TComponent, TModel>(TModel model, Dictionary<string, object?> parameters) where TComponent : IComponent;
    }
}