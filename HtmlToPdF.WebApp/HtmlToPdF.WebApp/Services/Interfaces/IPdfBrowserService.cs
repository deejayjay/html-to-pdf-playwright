namespace HtmlToPdF.WebApp.Services.Interfaces;

public interface IPdfBrowserService
{
    ValueTask DisposeAsync();
    Task<byte[]> GeneratePdfFromHtmlAsync(string html);
}