using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class ShowSubscriptionsCommand : IBotCommand
{
    private readonly ShowSubscriptionsUseCase _useCase;

    public ShowSubscriptionsCommand(ShowSubscriptionsUseCase useCase)
    {
        _useCase = useCase;
    }
    
    public async Task ExecuteAsync(UserContext userContext, string _, IMessageSender sender, CancellationToken ct)
    {
        var userSubscription = await _useCase.ExecuteAsync(userContext.UserId, ct);
        var names = userSubscription.Select(c => $"• {c.DisplayName}");
        var response = $"*Вы подписаны на следующие категории:*\n{string.Join("\n", names)}";
        await sender.SendAsync(userContext.ChatId, response, ct);
    }}