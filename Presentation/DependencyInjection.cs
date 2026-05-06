using Presentation.Bots.Commands;
using Presentation.Bots.TelegramBot;
using Telegram.Bot;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        var botToken = configuration["TelegramToken"];
        if (string.IsNullOrEmpty(botToken))
        {
            throw new InvalidOperationException("Telegram Token not found in Environment Variables.");
        }

        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

        services.AddSingleton<TelegramBotHandler>();
        services.AddHostedService<TelegramBotBackgroundService>();

        services.AddScoped<StartCommand>();
        services.AddScoped<ShowCategoriesCommand>();
        services.AddScoped<SubscribeCommand>();
        return services;
    }
}