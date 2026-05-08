using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class LatestCommand : IBotCommand
{
    private readonly GetLatestArticlesUseCase _useCase;
    private const int DefaultCount = 3;
    private const int MaxCount = 8;   // TODO : В глобалы
    private const int MinCount = 1;

    public LatestCommand(GetLatestArticlesUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task ExecuteAsync(UserContext userContext, string args, IMessageSender sender, CancellationToken ct)
    {
        if (!int.TryParse(args.Trim(), out int count)) count = DefaultCount;

        if (count < MinCount || count > MaxCount) count = DefaultCount;

        var articles = await _useCase.ExecuteAsync(count, ct);

        if (!articles.Any())
        {
            await sender.SendAsync(userContext.ChatId, "Статей пока нет.", ct);
            return;
        }
        
        var title = "Последние новости";
        await sender.SendArticlesAsync(userContext.ChatId, title, articles, ct);
    }
}