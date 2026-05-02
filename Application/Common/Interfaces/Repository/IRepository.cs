using Domain.Entities;

namespace Application.Common.Interfaces.Repository;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
}