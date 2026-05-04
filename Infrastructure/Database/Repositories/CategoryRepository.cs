using Application.Common.Interfaces.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Database.Repositories;

public class CategoryRepository : EfRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext dbContext) : base(dbContext) {}
    
    public Task<List<Category>> GetActiveAsync(CancellationToken ct)
        =>  DbSet.Where(s => s.IsActive).ToListAsync(ct);

}