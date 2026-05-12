using Domain.Entities;

namespace Application.Common.Models;

public record GetArticlesForDigestQuery(
    DateTimeOffset StartDate,
    Guid[]  CategoriesId);