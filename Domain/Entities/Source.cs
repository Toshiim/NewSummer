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
    public bool IsActive { get; private set; }
    
    private readonly List<Article> _articles = new();
    public IReadOnlyCollection<Article> Articles => _articles.AsReadOnly();
    
    protected Source() {}
    
    public Source(string name, string siteUrl, string feedUrl)
    {
        Name = name;
        SiteUrl = siteUrl;
        FeedUrl = feedUrl;
        IsActive = true;
    }

    public Article AddArticle(string title, string originalUrl, DateTimeOffset? publicationDate) 
    {
        var article = new Article(title, originalUrl,  Id, publicationDate);
        _articles.Add(article);
        return article;
    }
}