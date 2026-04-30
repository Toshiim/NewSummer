using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class NewsConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.OriginalUrl).IsRequired();
        builder.HasIndex(n => n.OriginalUrl).IsUnique();
        builder.Property(n => n.Title);
        builder.Property(n => n.Summary);

        builder.HasMany<Category>()
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "NewsCategory",
                j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                j => j.HasOne<News>().WithMany().HasForeignKey("NewsId"));
    }
}
