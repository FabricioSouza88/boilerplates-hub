using Database.Interfaces;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static void SetPrimaryKey<T>(this EntityTypeBuilder<T> builder) where T : class, ILongIdentifiableEntity
    {
        builder.HasKey(m => m.Id);
    }

    public static void IsAuditableEntity<T>(
        this EntityTypeBuilder<T> builder, string? defaultUserName = null
        ) where T : class, IAuditableEntity
    {
        var defaultDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var defaultUser = defaultUserName;

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValue(defaultDate);

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasDefaultValue(defaultUser);

        builder.Property(x => x.ModifiedAt)
            .HasDefaultValue(defaultDate);

        builder.Property(x => x.ModifiedBy)
            .HasDefaultValue(defaultUser);
    }
}
