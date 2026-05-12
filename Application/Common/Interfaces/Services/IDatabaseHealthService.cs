namespace Application.Common.Interfaces.Services;

public interface IDatabaseHealthService
{
    Task<bool> IsDatabaseHealthyAsync();
}