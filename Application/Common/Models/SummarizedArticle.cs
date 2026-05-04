namespace Application.Common.Models;

public record SummarizedArticle(
    string summary,
    List<string> category);