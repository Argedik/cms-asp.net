using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Summary { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public bool IsPublished { get; set; } = false;
        public DateTime? PublishedAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Foreign Keys
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User Author { get; set; } = null!;
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;
    }
} 