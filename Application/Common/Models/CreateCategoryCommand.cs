namespace Application.Common.Models;

public record CreateCategoryCommand(
    string DisplayName,
    string Slug
);