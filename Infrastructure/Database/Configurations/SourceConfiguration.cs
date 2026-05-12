using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.Property(s => s.Name).IsRequired();
        builder.Property(s => s.FeedUrl).IsRequired();
        builder.Property(s => s.SiteUrl).IsRequired();
        builder.HasIndex(s => s.FeedUrl).IsUnique();
        
        builder.HasMany(s => s.Articles)
            .WithOne(a => a.Source)
            .HasForeignKey(n => n.SourceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
