using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Services;

public interface IScraperService
{
    Task<IEnumerable<ScrapedArticle>> ScrapeAsync(Source source, CancellationToken ct = default);
}