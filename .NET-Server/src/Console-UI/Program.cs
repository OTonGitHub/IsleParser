using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Playwright;

namespace ConsoleUI
{
    sealed class Program
    {
        static async Task Main(string[] args)
        {
            string appStart = "Running..";
            Console.WriteLine(appStart);

            // initialize Playwright
            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

            // .Rane() => {start, count} highest = start + count - 1
            List<int> articleIds = Enumerable.Range(54100, 50).ToList();

            // semaphore to limit concurrency
            int maxConcurrency = 5;
            var semaphore = new SemaphoreSlim(maxConcurrency);

            var tasks = articleIds.Select(async articleId =>
            {
                await semaphore.WaitAsync();
                try
                {
                    string url = $"https://adhadhu.com/article/{articleId}";
                    await ProcessArticleAsync(browser, url, articleId);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
            await browser.CloseAsync();
        }

        static async Task ProcessArticleAsync(IBrowser browser, string url, int articleId)
        {
            try
            {
                var page = await browser.NewPageAsync();
                var response = await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.Load });

                if (response.Status == 404 || response.Status == 500 || response.Headers.ContainsKey("Location"))
                {
                    Console.WriteLine($"Article {articleId} not found, redirected, or server error. Skipping...");
                    await page.CloseAsync();
                    return;
                }

                string content = await page.ContentAsync();
                await page.CloseAsync();

                // load into HAP
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(content);

                var pTags = htmlDoc.DocumentNode.SelectNodes("//p[@class='font-faseyha font-19 color-black text-right mb-4']");
                if (pTags != null)
                {
                    foreach (var pTag in pTags)
                    {
                        Console.WriteLine($"Article {articleId}: {pTag.InnerText.Trim()}");
                    }
                }
                else
                {
                    Console.WriteLine($"No paragraphs found in Article {articleId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing Article {articleId}: {ex.Message}");
            }
        }
    }
}
