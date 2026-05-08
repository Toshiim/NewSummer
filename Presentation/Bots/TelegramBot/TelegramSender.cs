using Application.Common.Models;
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

    public Task SendAsync(string recipientId, string text, CancellationToken ct) =>
         _bot.SendMessage(long.Parse(recipientId), text, ParseMode.Markdown, cancellationToken: ct);

    public Task SendArticlesAsync(string recipientId, string title, ArticleViewModel[] articles, CancellationToken ct)
    {
        var message = $" *{title}:* \n\n {string.Join("\n", articles.Select(a => 
            $"*{a.Title}*\n\n{a.Summary}\n\nЧитать в источнике: [{a.Source}]({a.OriginalUrl})\n\n |{string.Join("|", a.Categories)}|{new string('—', 30)}"))}";

        return SendAsync(recipientId, message, ct);
    }
}