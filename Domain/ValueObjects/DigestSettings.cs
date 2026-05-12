namespace Domain.ValueObjects;

public record DigestSettings
{
    public int ArticlesCount { get; init; }
    public TimeSpan TargetUtcTime { get; init; } 

    protected DigestSettings() { }

    internal DigestSettings(int newsCount, TimeSpan targetUtcTime)
    {
        if (newsCount is < 1 or > 50)
            throw new ArgumentException("Количество новостей: 1-50");

        if (targetUtcTime < TimeSpan.Zero || targetUtcTime >= TimeSpan.FromDays(1))
            throw new ArgumentException("Некорректное время суток.");

        ArticlesCount = newsCount;
        TargetUtcTime = targetUtcTime;
    }
    
    public static DigestSettings Default => new(6, TimeSpan.FromHours(9));
    
    public static DigestSettings CreateFromUserLocal(int articlesCount, TimeOnly userTime, TimeSpan userOffset)
    {
        var localTimeAsSpan = userTime.ToTimeSpan();
        
        var utcTime = localTimeAsSpan.Subtract(userOffset);

        if (utcTime < TimeSpan.Zero) utcTime = utcTime.Add(TimeSpan.FromDays(1));
        if (utcTime >= TimeSpan.FromDays(1)) utcTime = utcTime.Subtract(TimeSpan.FromDays(1));

        return new DigestSettings(articlesCount, utcTime);
    }
}