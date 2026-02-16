using HtmlToPdF.Shared;
using HtmlToPdF.WebApp.Components.Reports;
using HtmlToPdF.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HtmlToPdF.WebApp.Controllers;

[Route("[controller]")]
[ApiController]
public class ReportsController(IHtmlGeneratorService htmlGenerator, IPdfBrowserService pdfBrowserService) : ControllerBase
{
    [HttpPost("pdf")]
    public async Task<IActionResult> GeneratePdfReport(List<User> users)
    {
        var html = await GenerateHtmlForReportAsync(users);

        var pdfBytes = await pdfBrowserService.GeneratePdfFromHtmlAsync(html);

        var stream = new MemoryStream(pdfBytes);

        return File(stream, "application/pdf", "UserReport.pdf");
    }

    private async Task<string> GenerateHtmlForReportAsync(List<User> users)
    {
        var parameters = new Dictionary<string, object?>
        {
            { "Users", users }
        };
        return await htmlGenerator.GenerateAsync<UsersReport, List<User>>(users, parameters);
    }    
}
