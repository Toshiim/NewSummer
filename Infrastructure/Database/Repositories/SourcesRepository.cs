using Application.Common.Interfaces.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class SourcesRepository : EfRepository<Source>, ISourceRepository
{
    public SourcesRepository(AppDbContext dbContext) : base(dbContext){}
    
    public Task<List<Source>> GetSourcesAsync(CancellationToken ct) => DbContext.Sources.AsNoTracking().ToListAsync(ct);
}