using edzotermi_berlet_projekt.Models;
using Microsoft.EntityFrameworkCore;

namespace edzotermi_berlet_projekt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        public DbSet<Tag> Tagok { get; set; }
        public DbSet<BerletTipus> BerletTipusok { get; set; }
        public DbSet<Berlet> Berletek { get; set; }
        public DbSet<Belepes> Belepesek { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Berlet>()
                .HasOne(b => b.Tag)
                .WithMany(t => t.Berletek)
                .HasForeignKey(b => b.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Berlet>()
                .HasOne(b => b.BerletTipus)
                .WithMany(bt => bt.Berletek)
                .HasForeignKey(b => b.BerletTipusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Belepes>()
                .HasOne(b => b.Berlet)
                .WithMany(b => b.Belepesek)
                .HasForeignKey(b => b.BerletId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BerletTipus>()
                .Property(bt => bt.Ar)
                .HasPrecision(10, 2);
        }
    }
}
