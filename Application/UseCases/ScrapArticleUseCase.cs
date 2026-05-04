using Application.Common.Interfaces;
using Application.Common.Interfaces.Repository;

namespace Application.UseCases;

public class ScrapArticleUseCase
{
    private readonly ISourceRepository _sources;
    private readonly IArticleRepository _articles;
    private readonly IScraperService _scraper;
    private readonly ISummaryService _summarizer;
    private readonly IUnitOfWork _uow;

    public ScrapArticleUseCase(
        ISourceRepository sources,
        IArticleRepository articles,
        IScraperService scraper,
        ISummaryService summarizer,
        IUnitOfWork uow)
    {
        _sources = sources;
        _articles = articles;
        _scraper = scraper;
        _summarizer = summarizer;
        _uow = uow;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        var sources = await _sources.GetActiveAsync(ct);

        foreach (var source in sources)
        {
            var scraped = await _scraper.ScrapeAsync(source, ct);

            foreach (var item in scraped)
            {
                if (await _articles.ExistsByUrlAsync(item.Url, ct)) continue;

                var article = source.AddNews(item.Url);
                var summary = await _summarizer.SummarizeAsync(item.RawText, ct);
                article.Enrich(item.Title, summary, []);
                await _articles.AddAsync(article, ct);
            }
            await _uow.SaveChangesAsync();
        }
    }
}