namespace Application.Common.Interfaces;

public interface ISummaryService
{
    Task<string> SummarizeAsync(string text, CancellationToken ct = default);
}