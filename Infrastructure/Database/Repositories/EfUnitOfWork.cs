using Application.Common.Interfaces.Repository;

namespace Infrastructure.Database.Repositories;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public EfUnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync()
        => _dbContext.SaveChangesAsync();
}
