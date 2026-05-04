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
                       Analyze the provided article and perform two tasks:
                       1. Summarize the article in 2-3 sentences.
                       2. Categorize the article using ONLY the provided categories list.

                       Categories list: [{{categoriesString}}]

                       Respond ONLY with a JSON object.
                       The language of the summary and categories must match the article language where applicable.

                       Structure:
                       {
                           "summary": "your concise summary here",
                           "category": ["category1", "category2"]
                       }

                       Article:
                       {{trimmed}}
                       """,
            Stream = false,
            Options = new RequestOptions 
            {
                Temperature = 0.3f, 
                NumPredict = 500,   
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
                return new SummarizedArticle("Ошибка: пустой ответ модели", []);
            }

            var jsonMatch = ExtractJson(rawContent);
            
            var summarized = JsonSerializer.Deserialize<SummarizedArticle>(jsonMatch, _jsonOptions);

            if (summarized == null)
            {
                throw new JsonException("Failed to deserialize Ollama response");
            }

            _logger.LogInformation("Ollama success. Categories found: {Count}", summarized.category.Count);
            return summarized;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ollama summarization failed");
            return new SummarizedArticle($"Ошибка: {ex.Message}", []);
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