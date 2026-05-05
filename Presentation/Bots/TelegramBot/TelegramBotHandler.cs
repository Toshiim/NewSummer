using Presentation.Bots.Abstractions;
using Presentation.Bots.Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Presentation.Bots.TelegramBot;

public class TelegramBotHandler
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<TelegramBotHandler> _logger;

    public TelegramBotHandler(IServiceProvider sp, ILogger<TelegramBotHandler> logger)
    {
        _sp = sp;
        _logger = logger;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        if (update.Message?.Text is not { } text) return;

        var parts = text.Split(' ');
        var commandName = parts[0].ToLower();
        var args = string.Join(' ', parts.Skip(1));

        await using var scope = _sp.CreateAsyncScope();
        
        var sender = new TelegramSender(bot);

        try
        {
            IBotCommand? command = commandName switch
            {
                "/start" => scope.ServiceProvider.GetRequiredService<StartCommand>(),
                "/topics" => scope.ServiceProvider.GetRequiredService<ShowCategoriesCommand>(),
                _ => null
            };

            if (command != null)
            {
                var userContext = new UserContext(
                    Username: update.Message.From?.Username ?? "Unknown", 
                    ChatId: update.Message.Chat.Id.ToString(),
                    UserId: update.Message.From?.Id.ToString() ?? throw new InvalidOperationException("User ID is missing"),
                    Platform: "Telegram"
                );
                await command.ExecuteAsync(userContext, args, sender, ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при выполнении команды {Command}", commandName);
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, HandleErrorSource source, CancellationToken ct)
    {
        _logger.LogError(ex, "Ошибка Telegram API: {Source}", source);
        return Task.CompletedTask;
    }
}