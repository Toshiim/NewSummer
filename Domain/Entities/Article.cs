namespace Domain.Entities;

public class Article : BaseEntity
{
    public string? Title { get; protected set; }
    public string? Summary { get; protected set; }
    public Guid SourceId { get; protected set; }
    public string OriginalUrl { get; protected set; }
    public DateTime DateAdded { get; protected set; }
    
    private readonly List<Guid> _categoryIds = new();
    public IReadOnlyCollection<Guid> CategoryIds => _categoryIds.AsReadOnly();

    protected Article() {}

    internal Article(string originalUrl, Guid sourceId, DateTime dateAdded)
    {
        OriginalUrl = originalUrl;
        SourceId = sourceId;
        DateAdded = dateAdded;
    }
    
    public void Enrich(string title, string summary, IEnumerable<Guid> categoryIds)
    {
        Title = title;
        Summary = summary;
        _categoryIds.AddRange(categoryIds);
    }
}