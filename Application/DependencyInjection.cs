using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddScoped<GetArticlesUseCase>();
        services.AddScoped<GetSourcesUseCase>();
        services.AddScoped<CreateSourceUseCase>();
        services.AddScoped<ScrapArticleUseCase>();
        services.AddScoped<CreateCategoryUseCase>();
        services.AddScoped<RegisterSubscriberUseCase>();
        services.AddScoped<GetCategoriesUseCase>();
        services.AddScoped<SubscribeForCategoresUseCase>();
        return services;
    }
}