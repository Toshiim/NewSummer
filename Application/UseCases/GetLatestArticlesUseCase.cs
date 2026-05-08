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

    public async Task<ArticleViewModel[]> ExecuteAsync(int count, CancellationToken ct)
        =>  await _repository.GetLatestArticlesAsync(count, ct);
}