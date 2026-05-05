using Presentation.Bots.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Presentation.Bots.TelegramBot;

public class TelegramSender : IMessageSender
{
    private readonly ITelegramBotClient _bot;
    public TelegramSender(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task SendAsync(string recipientId, string text, CancellationToken ct) =>
        await _bot.SendMessage(long.Parse(recipientId), text, ParseMode.Markdown, cancellationToken: ct);
}