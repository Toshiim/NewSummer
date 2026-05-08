using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using OllamaSharp.Models;

namespace Infrastructure.AI;

public class OllamaSummarizer : ISummaryService
{
    private readonly IOllamaApiClient _client;
    private readonly ILogger<OllamaSummarizer> _logger;
    private readonly string _model;
    private readonly JsonSerializerOptions _jsonOptions;

    public OllamaSummarizer(
        IOllamaApiClient client,
        ILogger<OllamaSummarizer> logger,
        IConfiguration configuration)
    {
        _client = client;
        _logger = logger;
        _model = configuration["Ollama:SummarizerModel"] ?? throw new ArgumentNullException("Ollama:SummarizerModel");
        
        _jsonOptions = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };
    }

    public async Task<SummarizedArticle> SummarizeAsync(string text, List<string> categories, CancellationToken ct = default)
    {
        var trimmed = text.Length > 10000 ? text[..10000] : text; // TODO : в appsettings
        var categoriesString = string.Join(", ", categories);
        
        var request = new GenerateRequest
        {
            Model = _model,
            Format = "json", 
            Prompt = $$"""
                       Проанализируй предоставленную статью и выполни задачи:
                       1. Сделай краткое резюме статьи (2-3 предложения).
                       2. Присвой статье категории, используя ТОЛЬКО предоставленный список категорий.
                       Список категорий: [{{categoriesString}}]
                       
                       3. Оцени общую важность события (score) по шкале от 1 до 10.
                       
                       Шкала оценки важности (score):
                       1-3: Низкая. Узкоспециализированная новость, рядовое событие, малозначительный анонс или гайд.
                       4-6: Средняя. Значимое событие в своей сфере, полезный материал для широкого круга заинтересованных лиц.
                       7-8: Высокая. Крупное событие, влияющее на целую индустрию или регион, важный анонс или открытие.
                       9-10: Критическая. Событие исторического масштаба, кардинально меняющее ситуацию в мире или крупной отрасли, обязательное к прочтению всем.
                       
                       Ответь СТРОГО в формате JSON.
                       Важно: Текст резюме и выбранные категории должны быть ВСЕГДА на русском языке, независимо от языка оригинала статьи.

                       Структура ответа:
                       {
                           "summary": "твое краткое резюме здесь",
                           "categories": ["категория1", "категория2"]
                           "score": целое число [1, 10]
                       }

                       Статья:
                       {{trimmed}}
                       """,
            Stream = false,
            Options = new RequestOptions 
            {
                Temperature = 0.3f, 
                NumPredict = 600,   
            }
        };

        try 
        {
            var response = await _client.GenerateAsync(request, ct).StreamToEndAsync();
            

            var rawContent = !string.IsNullOrWhiteSpace(response?.Response) // TODO: Костыль - Ollama с Qwen возвращает ответ в Thinking а не в Response 
                ? response.Response 
                : response?.Thinking;

            if (string.IsNullOrWhiteSpace(rawContent))
            {
                _logger.LogWarning("Ollama returned NOTHING. Model: {Model}", _model);
                return new SummarizedArticle("Ошибка: пустой ответ модели", [],0);
            }

            var jsonMatch = ExtractJson(rawContent);
            
            var summarized = JsonSerializer.Deserialize<SummarizedArticle>(jsonMatch, _jsonOptions);

            if (summarized == null)
            {
                throw new JsonException("Failed to deserialize Ollama response");
            }

            _logger.LogInformation("Ollama success. Categories found: {Count}", summarized.Categories.Count);
            return summarized;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ollama summarization failed");
            return new SummarizedArticle($"Ошибка: {ex.Message}",[],0 );
        }
    }

    /// <summary>
    /// Пытается найти JSON объект в строке, если модель вернула лишний текст
    /// </summary>
    private string ExtractJson(string input)
    {
        var startIndex = input.IndexOf('{');
        var endIndex = input.LastIndexOf('}');

        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
        {
            return input.Substring(startIndex, endIndex - startIndex + 1);
        }

        return input;
    }
}