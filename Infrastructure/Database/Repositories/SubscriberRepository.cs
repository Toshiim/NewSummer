using Application.Common.Interfaces.Repository;
using Domain.Entities;

namespace Infrastructure.Database.Repositories;

public class SubscriberRepository : EfRepository<Subscriber>, ISubscriberRepository
{
    public SubscriberRepository(AppDbContext dbContext) : base(dbContext) {}

}