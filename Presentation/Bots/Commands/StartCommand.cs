using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class StartCommand : IBotCommand
{
    private readonly RegisterSubscriberUseCase _useCase;

    public StartCommand(RegisterSubscriberUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task ExecuteAsync(UserContext userContext, string _, IMessageSender sender, CancellationToken ct)
    {
        await _useCase.ExecuteAsync(userContext.ChatId, userContext.Username, userContext.UserId, ct);

        var safeUsername = userContext.Username?.Replace("_", "\\_") ?? "User";
        var welcomeText = $"Привет, {safeUsername}! Ты в системе.";
        await sender.SendAsync(userContext.ChatId, welcomeText, ct);
    }
}