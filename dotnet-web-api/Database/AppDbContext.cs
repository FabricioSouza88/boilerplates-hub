using Microsoft.EntityFrameworkCore;
using StarterApp.Domain.Model;

namespace Database
{
    public class AppDbContext : BaseDbContext
    {
        public AppDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options, serviceProvider) { }

        public DbSet<SampleEntity> Samples { get; set; }
    }
}
