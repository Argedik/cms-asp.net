using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;

namespace CmsApi.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // .env dosyasını yükle
            Env.Load("../../.env");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Environment Variables'dan connection string oluştur
            var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                                  $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                                  $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                                  $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                                  $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";
            
            optionsBuilder.UseNpgsql(connectionString);
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
} 