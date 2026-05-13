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
        Math.Clamp(daysCountForDigest, 1, 14);
        var articles = await _useCase.ExecuteAsync(userContext.UserId, daysCountForDigest, ct);
        string daysWord = PluralHelper.GetPlural(daysCountForDigest, "сутки", "суток", "суток");
        var title = $"Твой Digest за {daysCountForDigest} {daysWord}.";
        await sender.SendArticlesAsync(userContext.ChatId, title, articles, ct);
    }
}
public static class PluralHelper
{
    public static string GetPlural(int number, string one, string two, string five)
    {
        int n = Math.Abs(number);
        n %= 100;
        if (n >= 5 && n <= 20) return five;
        n %= 10;
        if (n == 1) return one;
        if (n >= 2 && n <= 4) return two;
        return five;
    }
}