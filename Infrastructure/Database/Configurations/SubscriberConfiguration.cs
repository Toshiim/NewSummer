using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.Property(s => s.UserPlatformId).IsRequired();
        builder.HasIndex(s => s.UserPlatformId).IsUnique();
        builder.Property(s => s.ChatPlatformId).IsRequired();
        builder.Property(s => s.Username);

        builder.HasMany(s => s.Categories)
            .WithMany()
            .UsingEntity(j => j.ToTable("SubscriberCategories"));
    }
}