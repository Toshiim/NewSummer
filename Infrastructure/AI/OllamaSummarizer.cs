using Application.Common.Interfaces;
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

    public OllamaSummarizer(
        IOllamaApiClient client,
        ILogger<OllamaSummarizer> logger,
        IConfiguration configuration)
    {
        _client = client;
        _logger = logger;
        _model = configuration["Ollama:SummarizerModel"] ?? throw new ArgumentNullException("Ollama:SummarizerModel");
    }

    public async Task<string> SummarizeAsync(string text, CancellationToken ct = default)
    {
        var trimmed = text.Length > 10000 ? text[..10000] : text;

        var request = new GenerateRequest
        {
            Model = _model,
            Format = "json", 
            Prompt = $$"""
                       Summarize the following article in 2-3 sentences.
                       Respond ONLY with a JSON object.
                       The language of the summary must match the article language.

                       Structure:
                       {
                           "summary": "your concise summary here"
                       }

                       Article:
                       {{trimmed}}
                       """,
            Stream = false,
            Options = new RequestOptions 
            {
                Temperature = 0.3f,
                NumPredict = 200, 
                TopP = 0.9f
            }
        };

        try 
        {
            var response = await _client.GenerateAsync(request, ct).StreamToEndAsync();
            var result = response?.Thinking?.Trim(); // TODO: Жуткий костыль. Qwen из оламы возвращает ответ как размышление при любых условиях.

            if (string.IsNullOrWhiteSpace(result))
            {
                _logger.LogWarning("Ollama returned NOTHING. Check if model '{Model}' is pulled.", _model);
                return "ERROR_EMPTY_RESPONSE";
            }

            _logger.LogInformation("Ollama success. Summary length: {ResultLength}", result.Length);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ollama crashed!");
            return $"ERROR_EXCEPTION: {ex.Message}";
        }
    }
}