using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class SubscribeCommand : IBotCommand
{
    private readonly SubscribeForCategoresUseCase _useCase;

    public SubscribeCommand(SubscribeForCategoresUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task ExecuteAsync(UserContext userContext, string args, IMessageSender sender, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(args)) 
            throw new CommandValidationException($"""
                                                  📌 *Как подписаться на категории:*
                                                  Введите через пробел теги категорий.

                                                  Пример: `/subscribe world cats games`

                                                  _Список всех доступных тегов можно увидеть через /topics_
                                                  """);
        
        var tags = args.Split(new[] { ' ', ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
        
        var userSubscription = await _useCase.ExecuteAsync(userContext.UserId, tags, ct);
        var names = userSubscription.Select(c => $"• {c.DisplayName}");
        var response = $"*Теперь вы подписаны на следующие категории:*\n{string.Join("\n", names)}";
        await sender.SendAsync(userContext.ChatId, response, ct);
    }
}