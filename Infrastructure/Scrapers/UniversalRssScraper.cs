using System.ServiceModel.Syndication;
using System.Xml;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using SmartReader;
using Microsoft.Extensions.Logging; 

namespace Infrastructure.Scrapers;

public class UniversalRssScraper : IScraperService
{
    private readonly HttpClient _http;
    private readonly ILogger<UniversalRssScraper> _logger;

    public UniversalRssScraper(HttpClient http, ILogger<UniversalRssScraper> logger)
    {
        _http = http;
        _logger = logger;
        
        if (!_http.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        }
    }

    public async Task<IEnumerable<ScrapedArticle>> ScrapeAsync(Source source, CancellationToken ct = default)
    {
        _logger.LogInformation("Начинаем скрапинг ленты: {Url}", source.FeedUrl);

        var xml = await _http.GetStringAsync(source.FeedUrl, ct);
        using var xmlReader = XmlReader.Create(new StringReader(xml));
        var feed = SyndicationFeed.Load(xmlReader);

        var results = new List<ScrapedArticle>();

        var items = feed.Items.Take(5); // TODO : перерегать в appsettings

        foreach (var item in items)
        {
            var url = item.Links.FirstOrDefault()?.Uri?.ToString();
            if (string.IsNullOrWhiteSpace(url)) continue;

            try
            {
                var html = await _http.GetStringAsync(url, ct);

                var sr = new Reader(url, html);

                sr.CharThreshold = 100;     // TODO : перерегать в appsettings      
                sr.ContinueIfNotReadable = true;  
                
                sr.LoggerDelegate = msg => _logger.LogDebug("SmartReader: {Msg}", msg);

                var article = sr.GetArticle();

                if (string.IsNullOrWhiteSpace(article.TextContent))
                {
                    _logger.LogWarning("Статья пуста после парсинга: {Url}", url);
                    continue;
                }

                results.Add(new ScrapedArticle(
                    url, 
                    article.Title ?? item.Title.Text, 
                    article.TextContent));
                
                _logger.LogInformation("Успешно распаршено: {Title}", article.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке статьи {Url}", url);
                continue;
            }

            await Task.Delay(1000, ct);
        }

        return results;
    }
}