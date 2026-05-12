using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Repository;

public interface IArticleRepository: IRepository<Article>
{
    Task<PagedResult<ArticleDto>> GetPagedArticlesAsync(GetArticlesQuery query, CancellationToken ct);
    Task<bool> ExistsByUrlAsync(string url, CancellationToken ct);
    Task<ArticleViewModel[]> GetLatestArticlesAsync(int count, CancellationToken ct);
    Task<List<ArticleViewModel>> GetArticlesForDigestAsync(GetArticlesForDigestQuery query, CancellationToken ct);
}