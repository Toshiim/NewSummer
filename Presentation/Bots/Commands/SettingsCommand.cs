using Application.UseCases;
using Application.Common.Models;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class SettingsCommand : IBotCommand
{
    private readonly SetSettingsUseCase _useCase;
    
    public SettingsCommand(SetSettingsUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task ExecuteAsync(UserContext userContext, string? args, IMessageSender sender, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            throw new CommandValidationException($"""
                                                  📝 *Настройка дайджеста*
                                                  Пришлите данные через пробел:
                                                  1. Кол-во новостей (1-50)
                                                  2. Время дайджеста (ЧЧ:ММ)
                                                  3. Ваш текущий час (0-23)

                                                  Пример: `5 15:15 21`
                                                  """);
        }

        var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 3)
            throw new CommandValidationException("❌ Ошибка: нужно ввести 3 параметра.");

        if (!int.TryParse(parts[0], out var articlesCount))
            throw new CommandValidationException("❌ Некорректное число статей.");

        if (!TimeOnly.TryParse(parts[1], out var userTime))
            throw new CommandValidationException("❌ Некорректный формат времени (нужно ЧЧ:ММ).");

        if (!int.TryParse(parts[2], out var userCurrentHour) || userCurrentHour < 0 || userCurrentHour > 23)
            throw new CommandValidationException("❌ Текущий час должен быть от 0 до 23.");
        
        var utcNow = DateTimeOffset.UtcNow;
        var offsetHours = userCurrentHour - utcNow.Hour;
        var userOffset = TimeSpan.FromHours(offsetHours);
        var command = new SetSettingsCommand(articlesCount, userTime, userOffset);
        
        await _useCase.ExecuteAsync(userContext.UserId, command, ct);

        await sender.SendAsync(userContext.ChatId, "Настройки успешно обновлены", ct);
    }
    
}