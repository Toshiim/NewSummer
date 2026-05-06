namespace Presentation.Bots.Abstractions;

public interface IBotCommand
{
    Task ExecuteAsync(UserContext userContext, string args, IMessageSender sender, CancellationToken ct);
}