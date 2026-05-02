using Domain.Entities;

namespace Application.Common.Models;

public static class Mapping
{
    public static SourceResponse ToResponse(this Source source) =>
        new(
            source.Id,
            source.Name,
            source.SiteUrl,
            source.FeedUrl,
            source.CreatedAt,
            source.IsActive
            );
}