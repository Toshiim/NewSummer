namespace Presentation.Bots.Abstractions;

public interface IBotCommand
{
    Task ExecuteAsync(string chatId, string args, IMessageSender sender, CancellationToken ct);
}