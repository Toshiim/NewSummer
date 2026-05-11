namespace Domain.Entities;

public class Article : BaseEntity
{
    public string Title { get; protected set; }
    public string Summary { get; protected set; }
    public int ImportanceScore { get; protected set; }
    public Guid SourceId { get; protected set; }
    public virtual Source Source { get; protected set; }
    public string OriginalUrl { get; protected set; }
    public DateTimeOffset? PublicationDate { get; protected set; }
    
    private readonly List<Category> _categories = new();
    public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();

    protected Article() {}

    internal Article(string title, string originalUrl, Guid sourceId, DateTimeOffset? publicationDate)
    {
        Title = title;
        OriginalUrl = originalUrl;
        SourceId = sourceId;
        PublicationDate = publicationDate;
    }
    


    public void Enrich( string summary, int importanceScore, IEnumerable<Category> categories)
    {
        Summary = summary;
        ImportanceScore = importanceScore;
        _categories.AddRange(categories);
    }
}