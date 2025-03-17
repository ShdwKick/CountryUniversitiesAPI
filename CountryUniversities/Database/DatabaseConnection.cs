using CountryUniversities.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CountryUniversities.Database
{
    public class DatabaseConnection : DbContext
    {
        public DbSet<University> Universities { get; set; }
        public DbSet<WebPage> WebPages { get; set; }
        public DbSet<Domain> Domains { get; set; }

        public DatabaseConnection()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //TODO: брать из конфиг файла
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5433;Database=CountryUniversities;Username=postgres;Password=postgres");
            
        }

        //настройка связей между таблицами тспользуя FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<University>()
                .HasMany(u => u.WebPages)
                .WithOne(wp => wp.University)
                .HasForeignKey(wp => wp.UniversityId);

            modelBuilder.Entity<University>()
                .HasMany(u => u.Domains)
                .WithOne(d => d.University)
                .HasForeignKey(d => d.UniversityId);
        }
    }
}
