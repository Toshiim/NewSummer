using Application.Common.Interfaces;
using Application.Common.Interfaces.Repository;

namespace Application.UseCases;

public class ScrapArticleUseCase
{
    private readonly ISourceRepository _sources;
    private readonly IArticleRepository _articles;
    private readonly ICategoryRepository _categories; 
    private readonly IScraperService _scraper;
    private readonly ISummaryService _summarizer;
    private readonly IUnitOfWork _uow;

    public ScrapArticleUseCase(
        ISourceRepository sources,
        IArticleRepository articles,
        ICategoryRepository categories,
        IScraperService scraper,
        ISummaryService summarizer,
        IUnitOfWork uow)
    {
        _sources = sources;
        _articles = articles;
        _categories = categories;
        _scraper = scraper;
        _summarizer = summarizer;
        _uow = uow;
    }

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        var sources = await _sources.GetActiveAsync(ct);
        var allCategories = await _categories.GetActiveAsync(ct);
        var displayNames = allCategories.Select(c => c.DisplayName).ToList();

        foreach (var source in sources)
        {
            var scraped = await _scraper.ScrapeAsync(source, ct);

            foreach (var item in scraped)
            {
                if (await _articles.ExistsByUrlAsync(item.Url, ct)) continue;

                var article = source.AddArticle(item.Url);
                var summarizedArticle = await _summarizer.SummarizeAsync(item.RawText, displayNames , ct);
                
                var matchedCategories = allCategories
                    .Where(dbCat => summarizedArticle.category
                        .Any(aiCatName => aiCatName.Equals(dbCat.DisplayName, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
                
                article.Enrich(item.Title, summarizedArticle.summary, matchedCategories);
                await _articles.AddAsync(article, ct);
            }
            await _uow.SaveChangesAsync();
        }
    }
}