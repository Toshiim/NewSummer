namespace Application.Common.Models;

public record CreateSourceCommand(
    string Name,
    string SiteUrl,
    string FeedUrl
);