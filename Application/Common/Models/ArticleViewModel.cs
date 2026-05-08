namespace Application.Common.Models;

public record ArticleViewModel(
    string Title,
    string Summary,
    string Source,
    string OriginalUrl,
    string[] Categories
    );