namespace Application.Common.Models;

public record ArticleDto
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Summary { get; init; }
    public string OriginalUrl { get; init; } = null!;
    public DateTime DateAdded { get; init; }
    public Guid SourceId { get; init; }
}