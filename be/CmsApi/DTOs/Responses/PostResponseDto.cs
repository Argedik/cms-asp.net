namespace CmsApi.DTOs.Responses
{
    /// <summary>
    /// Post response DTO for API output
    /// SRP: Sadece post data presentation sorumluluğu
    /// Content management: Rich presentation with computed properties
    /// Performance: Optimized for different contexts (list vs detail)
    /// </summary>
    public class PostResponseDto
    {
        /// <summary>
        /// Post's unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Post title for display
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Post content - rich text/HTML
        /// Context: Full content for detail view
        /// Performance: May be truncated for list views
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Post excerpt/summary
        /// UX: Used in listing pages and previews
        /// Computed: Auto-generated if not provided
        /// </summary>
        public string Excerpt { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly slug for routing
        /// SEO: Used in clean URLs
        /// </summary>
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Featured image URL
        /// UX: Visual appeal in listings and social sharing
        /// </summary>
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// Content tags array (parsed from comma-separated string)
        /// Computed: Split and trimmed for easy frontend consumption
        /// </summary>
        public string[] Tags { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Publication status
        /// Business rule: Controls content visibility
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// When the post was published (null for drafts)
        /// Business rule: Actual publication timestamp
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Whether post is featured/highlighted
        /// UX: Featured posts get special treatment
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Whether comments are allowed
        /// Business rule: Controls comment functionality
        /// </summary>
        public bool AllowComments { get; set; }

        /// <summary>
        /// Post creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update timestamp
        /// UX: Show "last updated" to readers
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Category information
        /// Navigation: For breadcrumbs and filtering
        /// Performance: Joined data to avoid separate calls
        /// </summary>
        public CategorySummaryDto Category { get; set; } = new();

        /// <summary>
        /// Author information
        /// Context: Who wrote this content
        /// Privacy: Only public author data exposed
        /// </summary>
        public UserSummaryDto Author { get; set; } = new();

        /// <summary>
        /// Post metadata as parsed object
        /// Computed: JSON string converted to dynamic object
        /// Extensibility: Custom fields without schema changes
        /// </summary>
        public Dictionary<string, object>? MetaData { get; set; }

        /// <summary>
        /// SEO meta title for search engines
        /// SEO: Optimized for search results
        /// </summary>
        public string? MetaTitle { get; set; }

        /// <summary>
        /// SEO meta description
        /// SEO: Search result snippet
        /// </summary>
        public string? MetaDescription { get; set; }

        /// <summary>
        /// Estimated reading time in minutes
        /// Computed: Based on content length and average reading speed
        /// UX: Helps readers manage their time
        /// </summary>
        public int ReadingTimeMinutes { get; set; }

        /// <summary>
        /// Word count of the content
        /// Computed: Excluding HTML tags
        /// Analytics: Content length tracking
        /// </summary>
        public int WordCount { get; set; }

        /// <summary>
        /// Number of views/reads
        /// Analytics: Popularity metric
        /// Performance: Cached/computed value
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Number of likes/hearts
        /// Engagement: Social interaction metric
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// Number of comments on this post
        /// Engagement: Discussion activity metric
        /// Performance: Cached to avoid counting queries
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// Previous post in the series/category
        /// Navigation: Content discovery
        /// UX: "Previous post" functionality
        /// </summary>
        public PostNavigationDto? PreviousPost { get; set; }

        /// <summary>
        /// Next post in the series/category
        /// Navigation: Content discovery  
        /// UX: "Next post" functionality
        /// </summary>
        public PostNavigationDto? NextPost { get; set; }

        /// <summary>
        /// Related posts based on tags/category
        /// Discovery: Content recommendation
        /// Performance: Pre-computed relationships
        /// </summary>
        public List<PostSummaryDto>? RelatedPosts { get; set; }

        /// <summary>
        /// Whether current user has liked this post
        /// Context: User-specific interaction state
        /// Null if user not authenticated
        /// </summary>
        public bool? IsLikedByCurrentUser { get; set; }

        /// <summary>
        /// Whether current user can edit this post
        /// Authorization: Based on user role and ownership
        /// UX: Show/hide edit buttons
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Whether current user can delete this post
        /// Authorization: Based on user role and ownership
        /// UX: Show/hide delete buttons
        /// </summary>
        public bool CanDelete { get; set; }
    }

    /// <summary>
    /// Minimal user information for post references
    /// Performance: Avoid exposing full user data
    /// Privacy: Only public user information
    /// </summary>
    public class UserSummaryDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    /// <summary>
    /// Post navigation DTO for previous/next functionality
    /// UX: Streamlined navigation between posts
    /// Performance: Minimal data for navigation links
    /// </summary>
    public class PostNavigationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
    }
} 