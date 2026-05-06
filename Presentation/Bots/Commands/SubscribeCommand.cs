using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class SubscribeCommand : IBotCommand
{
    private readonly SubscribeForCategoresUseCase _useCase;

    public SubscribeCommand(SubscribeForCategoresUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task ExecuteAsync(UserContext userContext, string args, IMessageSender sender, CancellationToken ct)
    {
        var tags = args.Split(new[] { ' ', ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
        
        var userSubscription = await _useCase.ExecuteAsync(userContext.UserId, tags, ct);
        var names = userSubscription.Select(c => $"• {c.DisplayName}");
        var response = $"*Вы подписаны на следующие категории:*\n{string.Join("\n", names)}";
        await sender.SendAsync(userContext.ChatId, response, ct);
    }
}