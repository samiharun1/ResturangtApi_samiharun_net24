using Microsoft.EntityFrameworkCore;
using ResturangtApi_samiharun_net24.Models;

namespace ResturangtApi_samiharun_net24.Models.Data
{
    public class ResturangDbContext : DbContext
    {
        public ResturangDbContext(DbContextOptions<ResturangDbContext> options) : base(options)
        {
        }

        public DbSet<Bokning> Bokningar { get; set; } = null!;
        public DbSet<Kunder> Kunder { get; set; } = null!;
        public DbSet<Bord> Bord { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Meny> Meny { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bord>().HasIndex(t => t.BordNummer).IsUnique();
            modelBuilder.Entity<Admin>().HasIndex(a => a.Username).IsUnique();
            modelBuilder.Entity<Bokning>().HasIndex(x => new { x.BordId, x.StartTime });

            modelBuilder.Entity<Bokning>()
                .HasOne(b => b.Bord)
                .WithMany(b => b.Bokningar)
                .HasForeignKey(b => b.BordId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bokning>()
                .HasOne(b => b.Kunder)
                .WithMany(k => k.Bokningar)
                .HasForeignKey(b => b.KundId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}