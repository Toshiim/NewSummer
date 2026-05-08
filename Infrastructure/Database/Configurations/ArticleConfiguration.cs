using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.Property(n => n.OriginalUrl).IsRequired();
        builder.HasIndex(n => n.OriginalUrl).IsUnique();
        builder.Property(n => n.Title);
        builder.Property(n => n.Summary);
        builder.Property(n => n.ImportanceScore);
        builder.Property(a => a.PublicationDate)
            .IsRequired(false)
            .HasColumnType("timestamp with time zone");
        
        builder.HasMany(a => a.Categories)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "ArticleCategory",
                j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                j => j.HasOne<Article>().WithMany().HasForeignKey("NewsId"));
    }
}
