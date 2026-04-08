using Microsoft.EntityFrameworkCore;
using MORBIT_GNSS_APP.DataAccessLayer.Models;

namespace MORBIT_GNSS_APP.DataAccessLayer
{
    public class MorbitGnssAppDbContext : DbContext
    {
        // Constructor
        public MorbitGnssAppDbContext(DbContextOptions<MorbitGnssAppDbContext> options)
            : base(options)
        {
        }

        // Define DbSet properties
        public DbSet<GnssData> GnssData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GnssData>(entity =>
            {
                entity.ToTable("GnssData");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.GnssNmeaJson)
                      .IsRequired()
                      .HasColumnType("nvarchar(max)");

                entity.Property(x => x.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}