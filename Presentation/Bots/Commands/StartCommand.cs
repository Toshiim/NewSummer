using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class StartCommand : IBotCommand
{
    private readonly RegisterSubscriberUseCase _registerUseCase;

    public StartCommand(RegisterSubscriberUseCase registerUseCase)
    {
        _registerUseCase = registerUseCase;
    }

    public async Task ExecuteAsync(string chatId, string args, IMessageSender sender, CancellationToken ct)
    {
        await _registerUseCase.ExecuteAsync(chatId, null, null, ct);

        var welcomeText = "Привет! Ты успешно зарегистрирован в системе NewSummer.";
        await sender.SendAsync(chatId, welcomeText, ct);
    }
}