using Application.UseCases;

namespace Infrastructure.Jobs;

public class ScrapeNewsJob
{
    private readonly ScrapArticleUseCase _useCase;

    public ScrapeNewsJob(ScrapArticleUseCase useCase) => _useCase = useCase;

    public async Task ExecuteAsync() => await _useCase.ExecuteAsync();
}