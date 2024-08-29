using FileUploadApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FileUploadApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        private string BuildConnectionString()
        {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"C:\Users\todor.chankov\source\repos\UploadApp\UploadApp\appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            return connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
        BuildConnectionString(),
        options => options.UseAdminDatabase("postgres"));
        }

    }
}
