using Application.Common.Interfaces;
using Application.Common.Interfaces.Repository;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.AI;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Infrastructure.Scrapers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OllamaSharp;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, 
                b => b.MigrationsAssembly("Migrations")));

        services.AddScoped<IDatabaseHealthService, SqlDbHealthService>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ISourceRepository, SourcesRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        
        services.AddSingleton<IOllamaApiClient>(sp =>
        {
            var baseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
    
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromMinutes(3) // TODO : Перерегать в Appsettings
            };

            return new OllamaApiClient(httpClient);
        });
        services.AddScoped<ISummaryService, OllamaSummarizer>();
        
        services.AddHttpClient<IScraperService, UniversalRssScraper>(c =>
            c.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)"));

        services.AddHangfire(config => config
            .UsePostgreSqlStorage(connectionString));
        services.AddHangfireServer();
        
        return services;
    }
}