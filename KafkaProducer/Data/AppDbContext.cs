using KafkaProducer.Models;
using Microsoft.EntityFrameworkCore;

namespace KafkaProducer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SampleTable> SampleTables => Set<SampleTable>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleTable>(entity =>
            {
                entity.ToTable("SampleTable");
                entity.HasKey(e => e.UniqueId);
                entity.Property(e => e.UniqueId).ValueGeneratedOnAdd();
            });
        }
    }
}
