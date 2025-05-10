using Database;
using StarterApp.Domain.Model;

namespace dotnet_web_api
{
    public static class SeedData
    {
        public static async Task<IHost> SeedAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var samples = GetSamples();
                await context.Samples.AddRangeAsync(samples);
                await context.SaveChangesAsync();
            }

            return app;
        }

        private static IEnumerable<SampleEntity> GetSamples()
        {
            return Enumerable.Range(0, 10).Select(i => new SampleEntity()
            {
                Name = $"Sample {i}",
                Quantity = i  * 10,
                Date = DateTime.Now.AddDays(i),
                Active = true
            });
        }
    }
}
