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

        var welcomeText = $"Привет {userContext.Username}! Ты успешно зарегистрирован в системе NewSummer.";
        await sender.SendAsync(userContext.ChatId, welcomeText, ct);
    }
}