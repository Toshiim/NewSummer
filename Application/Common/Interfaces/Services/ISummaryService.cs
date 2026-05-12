using Application.Common.Models;

namespace Application.Common.Interfaces.Services;

public interface ISummaryService
{
    Task<SummarizedArticle> SummarizeAsync(string text, List<string> categories, CancellationToken ct = default);
}