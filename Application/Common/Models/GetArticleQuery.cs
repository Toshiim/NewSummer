namespace Application.Common.Models;

public record GetArticlesQuery(
    int Page,
    int PageSize,
    SortBy SortBy,
    SortOrder SortOrder,
    Guid? SourceId,
    Guid? CategoryId,
    string? SourceName
);

public enum SortOrder { Asc, Desc }

public enum SortBy {DateAdded, Title}