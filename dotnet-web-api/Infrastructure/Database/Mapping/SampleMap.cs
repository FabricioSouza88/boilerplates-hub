using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApp.Domain.Model;
using StarterApp.Infrastructure.Extensions;

namespace StarterApp.Infrastructure.Mapping
{
    public class SampleMap : IEntityTypeConfiguration<Sample>
    {
        public void Configure(EntityTypeBuilder<Sample> builder)
        {
            builder.SetPrimaryKey();
        }
    }
}
