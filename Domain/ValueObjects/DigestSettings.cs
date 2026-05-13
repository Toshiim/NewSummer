namespace Domain.ValueObjects;

public record DigestSettings
{
    public int ArticlesCount { get; init; }
    public TimeSpan TargetUtcTime { get; init; } 
    public TimeSpan UserOffset { get; init; }

    protected DigestSettings() { }

    internal DigestSettings(int articlesCount, TimeSpan targetUtcTime,  TimeSpan userOffset)
    {
        if (articlesCount is < 1 or > 50)
            throw new ArgumentException("Количество статей: 1-50");

        if (targetUtcTime < TimeSpan.Zero || targetUtcTime >= TimeSpan.FromDays(1))
            throw new ArgumentException("Некорректное время суток.");

        ArticlesCount = articlesCount;
        TargetUtcTime = targetUtcTime;
        UserOffset = userOffset;
    }
    
    internal static DigestSettings Default => new(6, TimeSpan.FromHours(9), TimeSpan.FromHours(3));
    
    internal static DigestSettings CreateFromUserLocal(int articlesCount, TimeOnly userTime, TimeSpan userOffset)
    {
        var localTimeAsSpan = userTime.ToTimeSpan();
        
        var utcTime = localTimeAsSpan.Subtract(userOffset);

        if (utcTime < TimeSpan.Zero) utcTime = utcTime.Add(TimeSpan.FromDays(1));
        if (utcTime >= TimeSpan.FromDays(1)) utcTime = utcTime.Subtract(TimeSpan.FromDays(1));

        return new DigestSettings(articlesCount, utcTime, userOffset);
    }
}