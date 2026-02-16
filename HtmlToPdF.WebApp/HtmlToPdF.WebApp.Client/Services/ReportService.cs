using HtmlToPdF.Shared;
using HtmlToPdF.WebApp.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace HtmlToPdF.WebApp.Client.Services;

public class ReportService: IReportService
{
    private readonly HttpClient _httpClient;

    public ReportService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("API");
    }

    public async Task<byte[]> GeneratePdfReportAsync(List<User> users)
    {
        var uri = "https://localhost:7273/reports/pdf";

        var response = await _httpClient.PostAsJsonAsync(uri, users);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsByteArrayAsync();
    }
}