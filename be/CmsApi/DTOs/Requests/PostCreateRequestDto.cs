using System.ComponentModel.DataAnnotations;

namespace CmsApi.DTOs.Requests
{
    /// <summary>
    /// Post creation request DTO
    /// SRP: Sadece post creation input data sorumluluğu
    /// Content management: Rich content with metadata
    /// Business rules: Publication workflow, SEO optimization
    /// </summary>
    public class PostCreateRequestDto
    {
        /// <summary>
        /// Post title - main heading for the content
        /// Business rules: Unique, SEO-optimized, engaging
        /// </summary>
        [Required(ErrorMessage = "Post title is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Post title must be between 5 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Post content - main body with rich text support
        /// Business rule: Support HTML/Markdown, minimum content length
        /// </summary>
        [Required(ErrorMessage = "Post content is required")]
        [StringLength(50000, MinimumLength = 50, ErrorMessage = "Post content must be between 50 and 50,000 characters")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Post excerpt/summary for listing pages
        /// Business rule: Auto-generated from content if not provided
        /// SEO: Used in meta descriptions and social sharing
        /// </summary>
        [StringLength(500, ErrorMessage = "Post excerpt cannot exceed 500 characters")]
        public string? Excerpt { get; set; }

        /// <summary>
        /// Category assignment for content organization
        /// Business rule: Must reference existing, active category
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid category ID is required")]
        public int CategoryId { get; set; }

        /// <summary>
        /// URL-friendly slug for SEO optimization
        /// Business rule: Auto-generated from title if not provided
        /// SEO: Clean URLs like /posts/my-awesome-post
        /// </summary>
        [StringLength(200, ErrorMessage = "Post slug cannot exceed 200 characters")]
        [RegularExpression(@"^[a-z0-9\-]+$", 
            ErrorMessage = "Post slug can only contain lowercase letters, numbers, and hyphens")]
        public string? Slug { get; set; }

        /// <summary>
        /// Featured image URL for visual appeal
        /// Business rule: Must be valid image URL
        /// UX: Used in listings and social sharing
        /// </summary>
        [StringLength(500, ErrorMessage = "Featured image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Featured image must be a valid URL")]
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// Content tags for enhanced discoverability
        /// Business rule: Comma-separated, max 10 tags
        /// SEO: Improves content categorization
        /// </summary>
        [StringLength(200, ErrorMessage = "Tags cannot exceed 200 characters")]
        public string? Tags { get; set; }

        /// <summary>
        /// Post metadata as JSON for extensibility
        /// Examples: {"readTime": 5, "difficulty": "beginner", "series": "Web Dev 101"}
        /// </summary>
        [StringLength(2000, ErrorMessage = "Post metadata cannot exceed 2000 characters")]
        public string? MetaData { get; set; }

        /// <summary>
        /// SEO meta title (different from post title)
        /// SEO: Optimized for search engines
        /// </summary>
        [StringLength(60, ErrorMessage = "Meta title cannot exceed 60 characters")]
        public string? MetaTitle { get; set; }

        /// <summary>
        /// SEO meta description for search results
        /// SEO: Displayed in search engine results
        /// </summary>
        [StringLength(160, ErrorMessage = "Meta description cannot exceed 160 characters")]
        public string? MetaDescription { get; set; }

        /// <summary>
        /// Publication status control
        /// Values: "Draft", "Published", "Scheduled", "Archived"
        /// Business rule: Default to "Draft" for content review
        /// </summary>
        [Required(ErrorMessage = "Publication status is required")]
        [RegularExpression("^(Draft|Published|Scheduled|Archived)$", 
            ErrorMessage = "Status must be: Draft, Published, Scheduled, or Archived")]
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Scheduled publication date (for "Scheduled" status)
        /// Business rule: Must be future date if status is "Scheduled"
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Whether post is featured/highlighted
        /// Business rule: Featured posts appear prominently
        /// Admin only: Regular users cannot feature posts
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Whether comments are allowed on this post
        /// Business rule: Default true, can be disabled per post
        /// </summary>
        public bool AllowComments { get; set; } = true;

        /// <summary>
        /// Display order for manual post sorting
        /// Business rule: Higher numbers appear first (if used)
        /// Default: 0 (chronological order)
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Display order must be a positive number")]
        public int DisplayOrder { get; set; } = 0;
    }
} 