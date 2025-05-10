using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StarterApp.Domain.Infrastructure;

namespace StarterApp.Infrastructure.Extensions
{
    public static class AuditableEntityEntension
    {
        public static void ApplyAuditableFields(this ChangeTracker changeTracker, Func<string>? getUserId = null, string defaultUser = "") 
        {
            var dateNow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            var userId = getUserId != null ? getUserId() : defaultUser;

            var addedEntities = changeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .OfType<IAuditableEntity>();

            foreach (var entity in addedEntities)
            {
                entity.CreatedAt = dateNow;
                entity.CreatedBy = userId;
                entity.ModifiedAt = dateNow;
                entity.ModifiedBy = userId;
            }

            var modifiedEntities = changeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .OfType<IAuditableEntity>();

            foreach (var entity in modifiedEntities)
            {
                entity.ModifiedAt = dateNow;
                entity.ModifiedBy = userId;
            }
        }
    }
}
