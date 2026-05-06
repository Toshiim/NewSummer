using System.ComponentModel;

namespace Domain.Entities;

public class Category : BaseEntity 
{
    /// <summary>
    /// Для понятного отображения пользователю
    /// </summary>
    public string DisplayName  { get; private set; }
    
    /// <summary>
    /// Псеводоним внутри системы. ВСЕГДА ToLower
    /// </summary>
    public string Slug { get; private set; }
    
    /// <summary>
    /// Для мягкого удаления 
    /// </summary>
    public bool IsActive { get; private set; }

    public void Deactivate() => IsActive = false;

    protected Category() { }
    
    public Category(string displayName, string slug)
    {
        DisplayName = displayName;
        Slug = slug.ToLower();
        IsActive = true;
    }
}