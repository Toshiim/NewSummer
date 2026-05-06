using Application.Common.Interfaces.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class SubscriberRepository : EfRepository<Subscriber>, ISubscriberRepository
{
    public SubscriberRepository(AppDbContext dbContext) : base(dbContext) {}

    public Task<Subscriber?> GetByPlatformIdAsync(string platformId, CancellationToken ct) 
        => DbSet
            .Include(s => s.Categories)
            .FirstOrDefaultAsync(s => s.UserPlatformId == platformId, ct);
}