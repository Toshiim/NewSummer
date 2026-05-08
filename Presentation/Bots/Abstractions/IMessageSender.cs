using Application.Common.Models;

namespace Presentation.Bots.Abstractions;

public interface IMessageSender
{
    Task SendAsync(string recipientId, string text, CancellationToken ct);
    Task SendArticlesAsync(string recipientId, string title, ArticleViewModel[] articles, CancellationToken ct);
}