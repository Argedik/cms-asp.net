using Microsoft.EntityFrameworkCore;
using CmsApi.Models;

namespace CmsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet'ler
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
            
            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
            });
            
            // Post configuration
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                
                entity.HasOne(p => p.Author)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Posts)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 