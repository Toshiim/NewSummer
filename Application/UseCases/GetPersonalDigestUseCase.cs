using Application.Common.Interfaces;
using Application.Common.Interfaces.Repository;
using Application.Common.Models;

namespace Application.UseCases;

public class GetPersonalDigestUseCase
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly IDigestFiltrationService _digestFiltrationService;

    public GetPersonalDigestUseCase(ISubscriberRepository repository,
        IArticleRepository articleRepository,
        IDigestFiltrationService digestFiltrationService)
    {
        _subscriberRepository = repository;
        _articleRepository = articleRepository;
        _digestFiltrationService = digestFiltrationService;
    }

    public async Task<ArticleViewModel[]> ExecuteAsync(string userId, int daysCountForDigest, CancellationToken ct)
    {
        var subscriber = await _subscriberRepository.GetByPlatformIdAsync(userId, ct) 
                         ?? throw new InvalidOperationException($"Пользователь {userId} не найден");
        
        var startTime = DateTimeOffset.UtcNow.AddDays(-daysCountForDigest); 
        var query = new GetArticlesForDigestQuery(startTime, subscriber.Categories.Select(c => c.Id).ToArray());
        var candidateArticles = await _articleRepository.GetArticlesForDigestAsync(query, ct);
        var digest = _digestFiltrationService.FilterCandidates(candidateArticles, subscriber.Categories, subscriber.Settings.ArticlesCount);

        return digest.ToArray();
    }
    
}