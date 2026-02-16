using HtmlToPdF.WebApp.Services.Interfaces;
using Microsoft.Playwright;

namespace HtmlToPdF.WebApp.Services;

public class PdfBrowserService : IPdfBrowserService, IAsyncDisposable
{
    private IPlaywright? _playwright;
    private readonly Task<IBrowser> _browserTask;

    public PdfBrowserService()
    {
        _browserTask = InitAsync();
    }

    private async Task<IBrowser> InitAsync()
    {
        //EnsurePlaywrightInstalled();

        _playwright = await Playwright.CreateAsync();

        return await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    public async Task<byte[]> GeneratePdfFromHtmlAsync(string html)
    {
        var browser = await _browserTask;

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            JavaScriptEnabled = false // if HTML doesn’t need JS
        });


        var page = await context.NewPageAsync();

        await page.SetContentAsync(html, new PageSetContentOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        var pdfBytes = await page.PdfAsync(new PagePdfOptions
        {
            Format = "A4",
            DisplayHeaderFooter = true, // Must be true for templates to work
            FooterTemplate = @"
                    <div style='font-size: 10px; width: 100%; text-align: center; font-family: sans-serif;'>
                        Page <span class='pageNumber'></span> of <span class='totalPages'></span>
                    </div>",
            Margin = new Margin
            {
                Top = "36px",
                Bottom = "36px",
                Left = "36px",
                Right = "36px"
            }
        });

        await context.CloseAsync();

        return pdfBytes;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_browserTask != null)
            {
                var browser = await _browserTask;
                await browser.CloseAsync();
            }

            _playwright?.Dispose();
        }
        catch { }
    }


    private static void EnsurePlaywrightInstalled()
    {
        var exitCode = Microsoft.Playwright.Program.Main(["install", "chromium"]);

        if (exitCode != 0)
        {
            throw new InvalidOperationException("Playwright browser installation failed.");
        }
    }
}