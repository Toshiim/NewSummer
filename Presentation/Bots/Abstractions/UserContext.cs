namespace Presentation.Bots.Abstractions;

public record UserContext(
    string Username,
    string ChatId,
    string UserId,
    string Platform);