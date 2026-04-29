namespace Domain.Entities;

public class News : BaseEntity
{
    public string? Title { get; protected set; }
    public string? Summary { get; protected set; }
    public Guid SourceId { get; protected set; }
    public string OriginalUrl { get; protected set; }
    public DateTime DateAdded { get; protected set; }
    
    protected News() {}

    internal News(string originalUrl, Guid sourceId, DateTime dateAdded)
    {
        OriginalUrl = originalUrl;
        SourceId = sourceId;
        DateAdded = dateAdded;
    }
    
    public void SetSummary(string summary)
    {
        Summary = summary;
    }
    
    public void SetTitle(string title)
    {
        Title = title;
    }
}