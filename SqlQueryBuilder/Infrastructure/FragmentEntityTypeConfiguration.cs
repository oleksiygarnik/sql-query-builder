using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlQueryBuilder.Domain;

namespace SqlQueryBuilder.Infrastructure
{
    public class FragmentEntityTypeConfiguration : IEntityTypeConfiguration<Fragment>
    {
        public void Configure(EntityTypeBuilder<Fragment> builder)
        {
            builder.ToTable(nameof(Fragment));
            builder.HasKey(c => c.Id);

            builder.HasOne<VirtualTable>()
                .WithMany();
        }
    }
}