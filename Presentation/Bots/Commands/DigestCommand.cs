using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class DigestCommand : IBotCommand
{
    private readonly GetPersonalDigestUseCase _useCase;

    public DigestCommand(GetPersonalDigestUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task ExecuteAsync(UserContext userContext, string args, IMessageSender sender, CancellationToken ct) 
    {
        int daysCountForDigest = int.TryParse(args, out var parsed) ? parsed : 1;
        if(daysCountForDigest < 1) daysCountForDigest = 1;
        if(daysCountForDigest >14)  daysCountForDigest = 14;
        var articles = await _useCase.ExecuteAsync(userContext.UserId, daysCountForDigest, ct);
        var title = "Твой Digest на сегодня";
        await sender.SendArticlesAsync(userContext.ChatId, title, articles, ct);
    }
}