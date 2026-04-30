using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.UserPlatformId).IsRequired();
        builder.HasIndex(s => s.UserPlatformId).IsUnique();
        builder.Property(s => s.Username);
    }
}