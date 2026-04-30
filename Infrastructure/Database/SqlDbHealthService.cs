using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
namespace Infrastructure.Database;

public class SqlDbHealthService(AppDbContext db) : IDatabaseHealthService
{
    public async Task<bool> IsDatabaseHealthyAsync()
    {
        try
        {
            await db.Database.ExecuteSqlRawAsync("SELECT 1");
            return true;
        }
        catch
        {
            return false;
        }
    }
}