using Presentation.Bots.Abstractions;
using Telegram.Bot;

namespace Presentation.Bots.TelegramBot;

public class TelegramSender : IMessageSender
{
    private readonly ITelegramBotClient _bot;
    public TelegramSender(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task SendAsync(string recipientId, string text, CancellationToken ct) =>
        await _bot.SendMessage(long.Parse(recipientId), text, cancellationToken: ct);
}