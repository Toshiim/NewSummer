namespace Application.Common.Models;

public record ArticleViewModel(
    Guid Id, 
    string Title,
    string Summary,
    string Source,
    string OriginalUrl,
    string[] Categories,
    Guid[] CategoryIds, 
    int ImportanceScore
);