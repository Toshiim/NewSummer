using Application.Common.Interfaces.Repository;
using Application.Common.Models;
using Domain.Entities;

namespace Application.UseCases;

public class GetSourcesUseCase
{
    protected ISourceRepository _repository;
    
    public GetSourcesUseCase(ISourceRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SourceResponse>> GetSources(CancellationToken ct)
    {
        var sources = await _repository.GetSourcesAsync(ct);
        return sources.Select(c => c.ToResponse()).ToList();
    }
}