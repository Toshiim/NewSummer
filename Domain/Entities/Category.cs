namespace Domain.Entities;

public class Category : BaseEntity  // не AggregateRoot
{
    /// <summary>
    /// Для понятного отображения пользователю
    /// </summary>
    public string DisplayName  { get; private set; }
    
    /// <summary>
    /// Псеводоним внутри системы
    /// </summary>
    public string Slug { get; private set; }
    
    /// <summary>
    /// Для мягкого удаления 
    /// </summary>
    public bool IsActive { get; private set; }

    public void Deactivate() => IsActive = false;
}