namespace Domain.Entities;

public class Article : BaseEntity
{
    public string? Title { get; protected set; }
    public string? Summary { get; protected set; }
    public Guid SourceId { get; protected set; }
    public string OriginalUrl { get; protected set; }
    public DateTime DateAdded { get; protected set; }
    
    private readonly List<Category> _categories = new();
    public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();

    protected Article() {}

    internal Article(string originalUrl, Guid sourceId, DateTime dateAdded)
    {
        OriginalUrl = originalUrl;
        SourceId = sourceId;
        DateAdded = dateAdded;
    }
    


    public void Enrich(string title, string summary, IEnumerable<Category> categories)
    {
        Title = title;
        Summary = summary;
        _categories.AddRange(categories);
    }
}