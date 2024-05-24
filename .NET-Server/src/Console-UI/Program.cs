using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Playwright;

sealed class Program
{
    static async Task Main(string[] args)
    {
        string appStart = "Running..";
        Console.WriteLine(appStart);

        // Initialize Playwright
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();

        string url = "https://adhadhu.com/article/54134";
        await page.GotoAsync(url);

        string content = await page.ContentAsync();

        // Load into HAP
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content);

        var pTags = htmlDoc.DocumentNode.SelectNodes("//p[@class='font-faseyha font-19 color-black text-right mb-4']");
        if (pTags != null)
        {
            foreach (var pTag in pTags)
            {
                Console.WriteLine(pTag.InnerText.Trim());
            }
        }
        else
        {
            throw new Exception("Unable To Find Paragraphs In Article");
        }

        await browser.CloseAsync();
    }
}