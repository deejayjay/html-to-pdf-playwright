using HtmlToPdF.Shared;

namespace HtmlToPdF.WebApp.Client.Services.Interfaces;

public interface IReportService
{
    Task<byte[]> GeneratePdfReportAsync(List<User> users);
}