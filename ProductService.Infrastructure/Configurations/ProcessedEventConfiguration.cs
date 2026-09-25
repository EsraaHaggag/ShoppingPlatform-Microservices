using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Configurations
{
    public class ProcessedEventConfiguration
    : IEntityTypeConfiguration<ProcessedEvent>
    {
        public void Configure(
            EntityTypeBuilder<ProcessedEvent> builder)
        {
            builder.HasKey(x => x.EventId);

            builder.Property(x => x.EventId)
                .ValueGeneratedNever();

            builder.Property(x => x.ProcessedAt)
                .IsRequired();
        }
    }
}
