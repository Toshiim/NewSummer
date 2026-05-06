using Domain.Entities;

namespace Application.Common.Interfaces.Repository;

public interface ISubscriberRepository : IRepository<Subscriber>
{
    Task<Subscriber> GetByPlatformIdAsync(string platformId, CancellationToken ct);
}