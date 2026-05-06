using Domain.Entities;

namespace Application.Common.Interfaces.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetActiveAsync(CancellationToken ct);
    Task<Category[]> GetByTags(string[] tags, CancellationToken ct);
}