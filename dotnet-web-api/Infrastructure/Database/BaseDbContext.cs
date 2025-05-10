using Microsoft.EntityFrameworkCore;
using StarterApp.Infrastructure.Extensions;
using System.Reflection;

namespace StarterApp.Infrastructure
{
    public abstract class BaseDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public BaseDbContext(DbContextOptions options, IServiceProvider? serviceProvider = null) : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        protected virtual void ApplySeed(ModelBuilder modelBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ApplySeed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ProcessAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessAudit()
        {
            var getUserFn = null as Func<string>;
            var defaultUser = "Anonymous";
            if (_serviceProvider != null)
            {
                getUserFn = () =>
                {
                    return defaultUser;
                };
            }

            ChangeTracker.ApplyAuditableFields(getUserFn, defaultUser);
        }
    }
}
