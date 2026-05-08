using Application.Common.Interfaces.Repository;
using Application.Common.Models;

namespace Application.UseCases;

public class GetLatestArticlesUseCase
{
    private readonly IArticleRepository _repository;

    public GetLatestArticlesUseCase(IArticleRepository repository)
    {
        _repository =  repository;
    }

    public Task<ArticleViewModel[]> ExecuteAsync(int count, CancellationToken ct)
        => _repository.GetLatestArticlesAsync(count, ct);
}