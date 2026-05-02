using Application.Common.Interfaces.Repository;
using Application.Common.Models;
using Domain.Entities;

namespace Application.UseCases;

public class GetArticlesUseCase
{
    private readonly IArticleRepository _repository;

    public GetArticlesUseCase(IArticleRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ArticleDto>> GetArticles(GetArticlesQuery  query, CancellationToken ct)
    {
        return await _repository.GetPagedArticlesAsync(query, ct);
    }
}