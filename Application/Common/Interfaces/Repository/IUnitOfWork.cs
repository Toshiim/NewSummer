namespace Application.Common.Interfaces.Repository;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}