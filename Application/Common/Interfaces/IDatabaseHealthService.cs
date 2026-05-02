namespace Application.Common.Interfaces;

public interface IDatabaseHealthService
{
    Task<bool> IsDatabaseHealthyAsync();
}