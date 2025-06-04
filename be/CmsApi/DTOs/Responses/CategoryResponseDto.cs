namespace CmsApi.DTOs.Responses
{
    /// <summary>
    /// Category response DTO for API output
    /// SRP: Sadece category data presentation sorumluluğu
    /// Hierarchical design: Tree structure with parent-child relationships
    /// Performance: Computed properties and aggregated data
    /// </summary>
    public class CategoryResponseDto
    {
        /// <summary>
        /// Category's unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Category name for display
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Category description for SEO and UX
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// URL-friendly slug for routing
        /// SEO: Used in clean URLs
        /// </summary>
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Parent category ID (null for root categories)
        /// Hierarchical: Indicates tree structure position
        /// </summary>
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// Parent category information (if exists)
        /// Navigation: Breadcrumb generation
        /// Performance: Avoid N+1 query problems
        /// </summary>
        public CategorySummaryDto? ParentCategory { get; set; }

        /// <summary>
        /// Child categories list
        /// Hierarchical: Direct children only (not recursive)
        /// Lazy loading: Can be null if not requested
        /// </summary>
        public List<CategorySummaryDto>? ChildCategories { get; set; }

        /// <summary>
        /// Display order for sorting
        /// Business rule: Controls category display sequence
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Whether category is active/visible
        /// Business rule: Affects content accessibility
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Category metadata as JSON string
        /// Extended: Keywords, color, icon, etc.
        /// </summary>
        public string? MetaData { get; set; }

        /// <summary>
        /// When the category was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When the category was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Number of posts in this category
        /// Computed: Aggregated from Posts table
        /// Performance: Cached value to avoid real-time counting
        /// </summary>
        public int PostCount { get; set; }

        /// <summary>
        /// Number of posts in this category and all subcategories
        /// Computed: Recursive count including hierarchy
        /// UX: Shows total content available
        /// </summary>
        public int TotalPostCount { get; set; }

        /// <summary>
        /// Full hierarchical path from root to this category
        /// Computed: "Technology > Programming > C#"
        /// UX: Breadcrumb navigation
        /// </summary>
        public string CategoryPath { get; set; } = string.Empty;

        /// <summary>
        /// Hierarchy level (0 = root, 1 = first level, etc.)
        /// Computed: For indentation and styling
        /// Business rule: May have maximum depth limit
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Whether this category has child categories
        /// Computed: For UI tree node rendering
        /// Performance: Avoid checking children existence repeatedly
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Latest post in this category (if any)
        /// Performance: Cached for homepage/category listing
        /// UX: Shows content freshness
        /// </summary>
        public PostSummaryDto? LatestPost { get; set; }
    }

    /// <summary>
    /// Simplified category DTO for hierarchical references
    /// Performance: Lighter payload for parent/child references
    /// SRP: Only essential category information
    /// </summary>
    public class CategorySummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int PostCount { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Minimal post information for category references
    /// Performance: Avoid circular references and heavy payloads
    /// UX: Just enough info for preview/navigation
    /// </summary>
    public class PostSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
} 