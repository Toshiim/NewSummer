using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

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