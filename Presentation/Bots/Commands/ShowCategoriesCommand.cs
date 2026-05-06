using Application.UseCases;
using Presentation.Bots.Abstractions;

namespace Presentation.Bots.Commands;

public class ShowCategoriesCommand : IBotCommand
{
    private readonly GetCategoriesUseCase _useCase;

    public ShowCategoriesCommand(GetCategoriesUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task ExecuteAsync(UserContext userContext, string args, IMessageSender sender, CancellationToken ct)
    {
        var categories = await _useCase.ExecuteAsync(ct);
        
        var categoryLines = categories
            .Select(c => $"`{c.Slug}` — **{c.DisplayName}**");

        var response = " *Список активных категорий:* \n\n" +
                       string.Join("\n", categoryLines) +
                       "\n\n_Для подписки на категории просто напиши тэг /subscribe и перечисли нужные_";    
        
        await sender.SendAsync(userContext.ChatId, response, ct);
    }
}