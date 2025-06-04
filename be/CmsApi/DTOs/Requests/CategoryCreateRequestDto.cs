using System.ComponentModel.DataAnnotations;

namespace CmsApi.DTOs.Requests
{
    /// <summary>
    /// Category creation request DTO
    /// SRP: Sadece category creation input data sorumluluğu
    /// Business rules: Name uniqueness, description validation
    /// Hierarchical design: Parent-child category relationships
    /// </summary>
    public class CategoryCreateRequestDto
    {
        /// <summary>
        /// Category name - must be unique and SEO-friendly
        /// Business rules: Used for URL generation, unique constraint
        /// </summary>
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_&]+$", 
            ErrorMessage = "Category name can only contain letters, numbers, spaces, hyphens, underscores, and ampersands")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category description for SEO and user understanding
        /// Business rule: Optional but recommended for better UX
        /// </summary>
        [StringLength(500, ErrorMessage = "Category description cannot exceed 500 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Parent category ID for hierarchical structure
        /// Business rule: Creates tree-like category organization
        /// Null = root category, Value = subcategory
        /// </summary>
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// Category slug for URL generation
        /// Business rule: Auto-generated from Name if not provided
        /// SEO: Clean URLs like /categories/technology-news
        /// </summary>
        [StringLength(100, ErrorMessage = "Category slug cannot exceed 100 characters")]
        [RegularExpression(@"^[a-z0-9\-]+$", 
            ErrorMessage = "Category slug can only contain lowercase letters, numbers, and hyphens")]
        public string? Slug { get; set; }

        /// <summary>
        /// Category metadata for SEO optimization
        /// JSON format: {"keywords": ["tech", "news"], "color": "#blue"}
        /// </summary>
        [StringLength(1000, ErrorMessage = "Category metadata cannot exceed 1000 characters")]
        public string? MetaData { get; set; }

        /// <summary>
        /// Display order for category sorting
        /// Business rule: Lower numbers appear first
        /// Default: 0 (auto-incremented by system)
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Display order must be a positive number")]
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Whether category is active/visible
        /// Business rule: Inactive categories hidden from public
        /// Default: true for new categories
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 