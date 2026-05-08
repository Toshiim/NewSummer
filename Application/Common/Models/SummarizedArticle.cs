using System.Text.Json.Serialization;

namespace Application.Common.Models;

public record SummarizedArticle(
    [property: JsonPropertyName("summary")] string Summary,
    [property: JsonPropertyName("categories")] List<string> Categories, 
    [property: JsonPropertyName("score")] int ImportanceScore
);