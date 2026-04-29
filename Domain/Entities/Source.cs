namespace Domain.Entities;

public class Source : BaseEntity
{
    public string Name { get; private set; }
    /// <summary>
    /// Основной URL источника
    /// </summary>
    public string SiteUrl { get; private set; }
    /// <summary>
    /// URL для парсинга
    /// </summary>
    public string FeedUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    
    private readonly List<News> _news = new();
    public IReadOnlyCollection<News> News => _news.AsReadOnly();
    
    protected Source() {}
    
    public Source(string name, string siteUrl, string feedUrl, DateTime createdAt, bool isActive)
    {
        Name = name;
        SiteUrl = siteUrl;
        FeedUrl = feedUrl;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public News AddNews(string originalUrl)
    {
        var news = new News(originalUrl,  Id, DateTime.UtcNow);
        _news.Add(news);
        return news;
    }
}