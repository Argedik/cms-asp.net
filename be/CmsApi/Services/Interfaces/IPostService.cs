using CmsApi.Models;

namespace CmsApi.Services.Interfaces
{
    /// <summary>
    /// Post business logic operations interface
    /// SRP: Post content management business logic sorumluluğu
    /// DIP: Abstract interface ile loose coupling
    /// OCP: Extensible for new post types and features
    /// Complex business rules: Publishing workflow, authorization, SEO, validation
    /// </summary>
    public interface IPostService
    {
        // POST CREATION - Complex Business Logic
        Task<(bool Success, Post? Post, string Message)> CreatePostAsync(string title, string content, int categoryId, int authorId);
        Task<(bool Success, Post? Post, string Message)> CreateDraftAsync(string title, string? content, int categoryId, int authorId);
        Task<(bool Success, Post? Post, string Message)> SaveDraftAsync(int postId, string? title, string? content, int? categoryId, int authorId);
        
        // POST RETRIEVAL - Business Logic with Authorization
        Task<Post?> GetPostByIdAsync(int id, int? requestUserId = null); // Authorization check
        Task<Post?> GetPostBySlugAsync(string slug, int? requestUserId = null);
        Task<Post?> GetPostWithDetailsAsync(int id, int? requestUserId = null); // Post + Author + Category
        Task<IEnumerable<Post>> GetAllPostsAsync(int? requestUserId = null);
        Task<IEnumerable<Post>> GetPublishedPostsAsync();
        
        // AUTHOR-SPECIFIC OPERATIONS - Authorization Business Logic
        Task<IEnumerable<Post>> GetUserPostsAsync(int userId, int? requestUserId = null); // Privacy control
        Task<IEnumerable<Post>> GetUserDraftsAsync(int userId, int requestUserId); // Author only
        Task<IEnumerable<Post>> GetUserPublishedPostsAsync(int userId);
        Task<int> GetUserPostCountAsync(int userId, bool includeInactive = false);
        
        // CATEGORY-BASED OPERATIONS - Business Logic
        Task<IEnumerable<Post>> GetPostsByCategoryAsync(int categoryId, bool includeInactive = false);
        Task<int> GetPostCountByCategoryAsync(int categoryId);
        Task<(bool Success, string Message)> ChangePostCategoryAsync(int postId, int newCategoryId, int requestUserId);
        
        // PUBLISHING WORKFLOW - State Management Business Logic
        Task<(bool Success, string Message)> PublishPostAsync(int postId, int requestUserId);
        Task<(bool Success, string Message)> UnpublishPostAsync(int postId, int requestUserId);
        Task<(bool Success, string Message)> ArchivePostAsync(int postId, int requestUserId);
        Task<(bool Success, string Message)> RestorePostAsync(int postId, int requestUserId);
        Task<bool> CanPublishPostAsync(int postId, int requestUserId);
        Task<bool> CanEditPostAsync(int postId, int requestUserId);
        
        // POST UPDATE - Complex Business Logic with Validation
        Task<(bool Success, Post? Post, string Message)> UpdatePostAsync(int postId, string? title, string? content, int? categoryId, int requestUserId);
        Task<(bool Success, string Message)> UpdatePostMetadataAsync(int postId, string? metaTitle, string? metaDescription, int requestUserId);
        
        // SLUG MANAGEMENT - Business Logic
        Task<string> GenerateSlugAsync(string title);
        Task<string> GenerateUniqueSlugAsync(string title, int? excludeId = null);
        Task<(bool Success, string Message)> UpdateSlugAsync(int postId, string newSlug, int requestUserId);
        
        // CONTENT VALIDATION - Business Rules
        Task<(bool IsValid, string Message)> ValidatePostContentAsync(string title, string content);
        Task<(bool IsValid, string Message)> ValidatePostForPublishingAsync(int postId);
        Task<bool> IsTitleAvailableAsync(string title, int? excludeId = null);
        Task<bool> IsSlugAvailableAsync(string slug, int? excludeId = null);
        
        // SEARCH & FILTERING - Advanced Business Logic
        Task<IEnumerable<Post>> SearchPostsAsync(string searchTerm, int? categoryId = null, int? authorId = null, bool publishedOnly = true);
        Task<IEnumerable<Post>> SearchInTitleAsync(string searchTerm, bool publishedOnly = true);
        Task<IEnumerable<Post>> SearchInContentAsync(string searchTerm, bool publishedOnly = true);
        Task<IEnumerable<Post>> GetPostsByDateRangeAsync(DateTime startDate, DateTime endDate, bool publishedOnly = true);
        
        // PAGINATION - Performance Critical Business Logic
        Task<IEnumerable<Post>> GetPagedPostsAsync(int pageNumber, int pageSize, int? categoryId = null, int? authorId = null, bool publishedOnly = true);
        Task<int> GetTotalPostCountAsync(int? categoryId = null, int? authorId = null, bool publishedOnly = true);
        Task<IEnumerable<Post>> GetRecentPostsAsync(int count, bool publishedOnly = true);
        Task<IEnumerable<Post>> GetPopularPostsAsync(int count); // Future: view count based
        
        // POST DELETION - Business Rules
        Task<(bool Success, string Message)> DeletePostAsync(int postId, int requestUserId);
        Task<(bool Success, string Message)> SoftDeletePostAsync(int postId, int requestUserId);
        Task<(bool Success, string Message)> PermanentDeletePostAsync(int postId, int requestUserId); // Admin only
        
        // BULK OPERATIONS - Advanced Business Logic
        Task<(int Success, int Failed, string Message)> BulkPublishPostsAsync(int[] postIds, int requestUserId);
        Task<(int Success, int Failed, string Message)> BulkUnpublishPostsAsync(int[] postIds, int requestUserId);
        Task<(int Success, int Failed, string Message)> BulkDeletePostsAsync(int[] postIds, int requestUserId);
        Task<(int Success, int Failed, string Message)> BulkChangeCategoryAsync(int[] postIds, int newCategoryId, int requestUserId);
        
        // STATISTICS & ANALYTICS - Business Intelligence
        Task<int> GetTotalPostCountAsync();
        Task<int> GetPublishedPostCountAsync();
        Task<int> GetDraftPostCountAsync();
        Task<Dictionary<string, int>> GetPostCountByStatusAsync();
        Task<Dictionary<string, int>> GetPostCountByMonthAsync(int year);
        Task<IEnumerable<Post>> GetTrendingPostsAsync(int count, int days = 30); // Future: analytics integration
        
        // SEO & METADATA - Business Logic
        Task<(bool Success, string Message)> OptimizePostForSEOAsync(int postId, int requestUserId);
        Task<string> GenerateMetaDescriptionAsync(string content, int maxLength = 160);
        Task<IEnumerable<string>> ExtractKeywordsAsync(string content, int maxKeywords = 10);
        Task<(bool IsOptimized, string[] Suggestions)> AnalyzeSEOAsync(int postId);
        
        // CONTENT MANAGEMENT - Advanced Business Logic  
        Task<(bool Success, string Message)> DuplicatePostAsync(int sourcePostId, int requestUserId);
        Task<(bool Success, string Message)> SchedulePostAsync(int postId, DateTime publishDate, int requestUserId);
        Task<IEnumerable<Post>> GetScheduledPostsAsync();
        Task<(bool Success, string Message)> ProcessScheduledPostsAsync(); // Background job için
        
        // COLLABORATION - Multi-user Business Logic
        Task<(bool Success, string Message)> TransferPostOwnershipAsync(int postId, int newOwnerId, int requestUserId);
        Task<IEnumerable<User>> GetPostContributorsAsync(int postId); // Future: co-authoring
        Task<(bool Success, string Message)> AddPostContributorAsync(int postId, int contributorId, int requestUserId);
    }
} 