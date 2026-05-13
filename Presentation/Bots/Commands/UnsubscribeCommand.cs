using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class UnsubscribeCommand : IBotCommand
{
    private readonly UnsubscribeForCategoresUseCase _useCase;

    public UnsubscribeCommand(UnsubscribeForCategoresUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task ExecuteAsync(UserContext userContext, string args, IMessageSender sender, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new CommandValidationException($"""
                                                  🔌 *Как отписаться:*
                                                  Введите теги категорий, которые вам больше не интересны.

                                                  Пример: `/unsubscribe politics war`

                                                  _Ваши текущие подписки можно глянуть через /mysubs_
                                                  """);
        
        var tags = args.Split(new[] { ' ', ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
        
        var userSubscription = await _useCase.ExecuteAsync(userContext.UserId, tags, ct);
        var names = userSubscription.Select(c => $"• {c.DisplayName}");
        var response = $"*Теперь вы подписаны на следующие категории:*\n{string.Join("\n", names)}";
        await sender.SendAsync(userContext.ChatId, response, ct);
    }
}