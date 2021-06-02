using Microsoft.EntityFrameworkCore;
using SqlQueryBuilder.Domain;

namespace SqlQueryBuilder.Infrastructure
{
    public sealed class BaseDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Fragment> Fragments { get; set; }
        public DbSet<VirtualTable> VirtualTables { get; set; }

        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CarEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FragmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VirtualTableEntityTypeConfiguration());
        }
    }
}
