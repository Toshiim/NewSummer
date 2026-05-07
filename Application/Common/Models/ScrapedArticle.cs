namespace Application.Common.Models;

public record ScrapedArticle(string Url, string Title, DateTimeOffset? PublicationTime, string RawText);