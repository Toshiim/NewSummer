namespace Application.Interfaces;

public interface IDatabaseHealthService
{
    Task<bool> IsDatabaseHealthyAsync();
}