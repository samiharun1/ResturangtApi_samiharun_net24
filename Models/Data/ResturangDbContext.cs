using Microsoft.EntityFrameworkCore;
using ResturangtApi_samiharun_net24.Models;

namespace ResturangtApi_samiharun_net24.Models.Data

//Den här filen skapar kopplingen mellan C#-modellerna och databasen.
// Den innehåller DbSet för varje modell:
// Bokningar
// Kunder
// Bord
// Admins
// Meny
//
// Den här filen styr också hur tabellerna är kopplade (relations):
// Bord och Bokning
// Kunder och Bokning
//
// Den ser till att vissa fält är unika och skapar index för snabbare sökningar.
// Exempel: inget bord kan ha samma nummer, och inga två admins kan ha samma användarnamn.
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
            // BordNummer måste vara unikt → inga två bord kan ha samma nummer

            modelBuilder.Entity<Admin>().HasIndex(a => a.Username).IsUnique();
            // Username måste vara unikt → inga två admins kan ha samma namn

            modelBuilder.Entity<Bokning>().HasIndex(x => new { x.BordId, x.StartTime });
            // Skapar index på BordId + StartTime för att snabbare söka och undvika exakt dubbelbokning


            modelBuilder.Entity<Bokning>()
                .HasOne(b => b.Bord)
                .WithMany(b => b.Bokningar)
                .HasForeignKey(b => b.BordId)
                .OnDelete(DeleteBehavior.Cascade);
            // En bokning hör till ett bord, när bordet tas bort tas bokningen också bort


            modelBuilder.Entity<Bokning>()
                .HasOne(b => b.Kunder)
                .WithMany(k => k.Bokningar)
                .HasForeignKey(b => b.KundId)
                .OnDelete(DeleteBehavior.Cascade);
            // En bokning hör till en kund, när kunden tas bort tas bokningen också bort

        }
    }
}