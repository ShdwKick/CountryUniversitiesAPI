using CountryUniversities.DataModels;
using CountryUniversities.Services;
using Microsoft.EntityFrameworkCore;

namespace CountryUniversities.Database
{
    public class DatabaseConnection : DbContext
    {
        private readonly string _connectionString;
        
        public DbSet<University> Universities { get; set; }
        public DbSet<WebPage> WebPages { get; set; }

        public DatabaseConnection(IConfiguration config, IUniversityService testServ)
        {
            _connectionString = config.GetSection("AppSettings:DefaultConnection").Value;
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_connectionString);
        }

        //настройка связей между таблицами тспользуя FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<University>()
                .HasMany(u => u.WebPages)
                .WithOne(wp => wp.University)
                .HasForeignKey(wp => wp.UniversityId);
        }
    }
}
