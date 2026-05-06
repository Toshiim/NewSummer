using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Presentation.Bots.TelegramBot;

public class TelegramBotBackgroundService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly TelegramBotHandler _handler;

    public TelegramBotBackgroundService(ITelegramBotClient botClient, TelegramBotHandler handler)
    {
        _botClient = botClient;
        _handler = handler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _botClient.StartReceiving(
            updateHandler: _handler.HandleUpdateAsync,
            errorHandler: _handler.HandleErrorAsync,
            receiverOptions: new ReceiverOptions { AllowedUpdates = [] },
            cancellationToken: stoppingToken
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}