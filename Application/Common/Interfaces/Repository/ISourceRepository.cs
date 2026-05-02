using Domain.Entities;

namespace Application.Common.Interfaces.Repository;

public interface ISourceRepository : IRepository<Source>
{
    Task<List<Source>> GetSourcesAsync(CancellationToken ct);
}