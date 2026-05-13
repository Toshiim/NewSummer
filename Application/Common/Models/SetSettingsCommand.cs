namespace Application.Common.Models;

public record SetSettingsCommand(
    int ArticlesCount,
    TimeOnly UserTime,
    TimeSpan UserOffset);