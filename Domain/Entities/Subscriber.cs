namespace Domain.Entities;

public class Subscriber : BaseEntity
{
    /// <summary>
    /// Nullable - не во всех системах можно определить username
    /// </summary>
    public string? Username { get; protected set; }
    
    /// <summary>
    /// Id на платформе. 
    /// </summary>
    /// <example>
    /// Telegram Id - 9 - 10 цифр
    /// Discord Id - Snowflake Id 64 бита
    /// Other - Guid, url, email etc
    /// Для объединения всех этих форматов использую string
    /// </example>>
    public string UserPlatformId  { get; protected set; }

    public string ChatPlatformId  { get; protected set; }
    public DateTime CreatedAt { get; protected set; }

    private readonly List<Category> _categories = new();
    public virtual IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();
    
    public Subscriber(string username, string userPlatformId, string chatPlatformId)
    {
        Username = username;
        UserPlatformId = userPlatformId;
        ChatPlatformId = chatPlatformId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void SubscribeTo(Category category)
    {
        if (!_categories.Contains(category))
        {
            _categories.Add(category);
        }
    }

    public void UnsubscribeFrom(Category category)
    {
        _categories.Remove(category);
    }
}