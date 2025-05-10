namespace StarterApp.Infrastructure
{
    public class AppDbContext : BaseDbContext
    {
        public AppDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options, serviceProvider) { }

        public DbSet<Sample> Sample { get; set; }
        public DbSet<SampleItem> SampleItem { get; set; }
    }
}
