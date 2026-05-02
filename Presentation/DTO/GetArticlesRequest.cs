using Application.Common.Models;

namespace Presentation.DTO;

using System.ComponentModel;

public record GetArticlesRequest(
    [DefaultValue(1)] int? Page,
    [DefaultValue(20)] int? PageSize,
    [DefaultValue(SortBy.DateAdded)] SortBy? SortBy,
    [DefaultValue(SortOrder.Desc)] SortOrder? SortOrder,
    Guid? SourceId,
    Guid? CategoryId,
    string? SourceName
);