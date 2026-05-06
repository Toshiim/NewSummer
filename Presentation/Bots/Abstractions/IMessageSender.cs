namespace Presentation.Bots.Abstractions;

public interface IMessageSender
{
    Task SendAsync(string recipientId, string text, CancellationToken ct);
}