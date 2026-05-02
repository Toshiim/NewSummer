namespace Application.Common.Models;

public record SourceResponse(
    Guid Id,
    string Name,
    string SiteUrl,
    string FeedUrl,
    DateTime CreatedAt,
    bool IsActive
    );