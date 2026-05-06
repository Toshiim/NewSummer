using Application.Common.Interfaces.Repository;
using Domain.Entities;

namespace Application.UseCases;

public class GetCategoriesUseCase
{
    private ICategoryRepository _repository;
    
    public GetCategoriesUseCase(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Category>> ExecuteAsync(CancellationToken ct)
    {
        return _repository.GetActiveAsync(ct);
    }
}