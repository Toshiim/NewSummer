using Application.Common.Interfaces.Repository;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class ArticleRepository : EfRepository<Article>, IArticleRepository
{
    public ArticleRepository(AppDbContext dbContext) : base(dbContext){}
    
    public Task<bool> ExistsByUrlAsync(string url, CancellationToken ct)
        =>  DbSet.AnyAsync(a => a.OriginalUrl == url, ct);
    
    public Task<Article[]> GetLatestArticlesAsync(int count, CancellationToken ct) 
        => DbSet
            .AsNoTracking()
            .OrderBy(a => a.PublicationDate == null)
            .ThenByDescending(a => a.PublicationDate)
            .Take(count)
            .ToArrayAsync(ct);
    
    public async Task<PagedResult<ArticleDto>> GetPagedArticlesAsync(
        GetArticlesQuery query, 
        CancellationToken ct = default)
    {
        IQueryable<Article> q = DbSet.AsNoTracking();

        if (query.SourceId.HasValue)
            q = q.Where(a => a.SourceId == query.SourceId.Value);

        if (!string.IsNullOrWhiteSpace(query.SourceName))
            q = q.Where(a => DbContext.Set<Source>()
                .Any(s => s.Id == a.SourceId && EF.Functions.ILike(s.Name, query.SourceName)));

        if (query.CategoryId.HasValue)
            q = q.Where(a => DbContext.Set<Dictionary<string, object>>("NewsCategory")
                .Any(nc => (Guid)nc["NewsId"] == a.Id && (Guid)nc["CategoryId"] == query.CategoryId.Value));

        q = (query.SortBy, query.SortOrder) switch
        {
            (SortBy.Title, SortOrder.Asc)             => q.OrderBy(a => a.Title),
            (SortBy.Title, SortOrder.Desc)            => q.OrderByDescending(a => a.Title),
            (SortBy.PublicationDate, SortOrder.Asc)   => q.OrderBy(a => a.PublicationDate),
            (SortBy.PublicationDate, SortOrder.Desc)  => q.OrderBy(a => a.PublicationDate == null).ThenByDescending(a => a.PublicationDate),
            (SortBy.CreatedAt, SortOrder.Asc)         => q.OrderBy(a => a.CreatedAt),
            (SortBy.CreatedAt, SortOrder.Desc)        => q.OrderByDescending(a => a.CreatedAt)
        };
        
        var totalCount = await q.CountAsync(ct); 

        var items = await q
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Summary = a.Summary,
                OriginalUrl = a.OriginalUrl,
                PublicationDate = a.PublicationDate,
                CreatedAt = a.CreatedAt,
                SourceId = a.SourceId
            })
            .ToListAsync(ct);

        return new PagedResult<ArticleDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}