using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlQueryBuilder.Domain;

namespace SqlQueryBuilder.Infrastructure
{
    public class VirtualTableEntityTypeConfiguration : IEntityTypeConfiguration<VirtualTable>
    {
        public void Configure(EntityTypeBuilder<VirtualTable> builder)
        {
            builder.ToTable(nameof(VirtualTable));
            builder.HasKey(c => c.Id);
        }
    }
}