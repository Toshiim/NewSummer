using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces.Repository;

public interface IArticleRepository: IRepository<Article>
{
    Task<PagedResult<ArticleDto>> GetPagedArticlesAsync(GetArticlesQuery query, CancellationToken ct);
    Task<bool> ExistsByUrlAsync(string url, CancellationToken ct);
}