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
    
    private readonly List<Article> _articles = new();
    public IReadOnlyCollection<Article> Articles => _articles.AsReadOnly();
    
    protected Source() {}
    
    public Source(string name, string siteUrl, string feedUrl)
    {
        Name = name;
        SiteUrl = siteUrl;
        FeedUrl = feedUrl;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public Article AddNews(string originalUrl) // TODO: rename
    {
        var news = new Article(originalUrl,  Id, DateTime.UtcNow);
        _articles.Add(news);
        return news;
    }
}