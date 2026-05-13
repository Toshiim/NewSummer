using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class MeCommand : IBotCommand
{
    GetSubscriberInfoUseCase _useCase;
    
    public MeCommand(GetSubscriberInfoUseCase useCase) => _useCase = useCase;
    
    public async Task ExecuteAsync(UserContext userContext, string _, IMessageSender sender, CancellationToken ct)
    {
        var user = await _useCase.ExecuteAsync(userContext.UserId, ct);
    
        if (user == null)
        {
            await sender.SendAsync(userContext.UserId, "Вас не существует :? \n\n напишите /start", ct);
            return;
        }

        string categoriesInfo = user.Categories.Any() 
            ? string.Join("\n", user.Categories.Select(c => $"  • {c.DisplayName}")) 
            : "  (подписок пока нет)";

        var safeUsername = user.Username?.Replace("_", "\\_") ?? "User";
        var localTime = user.Settings.TargetUtcTime.Add(user.Settings.UserOffset);
        string offsetSign = user.Settings.UserOffset.Hours >= 0 ? "+" : "";
        string displayTime = $"{localTime:hh\\:mm} (GMT{offsetSign}{user.Settings.UserOffset.Hours})";
        string text = $"""
                       👤 Профиль пользователя
                       ────────────────────────
                       Имя: {safeUsername}
                       ID платформы: {user.UserPlatformId}
                       ID чата: {user.ChatPlatformId}

                        Последний дайджест:
                       {(user.LastDigestSentAt.HasValue ? user.LastDigestSentAt.Value.ToString("f") : "Ещё не отправлялся")}

                        Ваши подписки:
                       {categoriesInfo}

                        Настройки:
                       Количество новостей: {user.Settings.ArticlesCount.ToString()}
                       Время рассылки: {displayTime}
                       ────────────────────────
                       """;

        await sender.SendAsync(userContext.UserId, text, ct);
    }
}