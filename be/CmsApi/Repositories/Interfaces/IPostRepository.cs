using CmsApi.Models;

namespace CmsApi.Repositories.Interfaces
{
    /// <summary>
    /// Post entity için data access operasyonlarını tanımlayan interface
    /// SRP: Sadece Post ile ilgili data access sorumluluğu
    /// DIP: Abstract interface ile loose coupling
    /// ISP: Interface Segregation - sadece gerekli metodlar
    /// </summary>
    public interface IPostRepository
    {
        // CREATE - Yeni post oluşturma
        Task<Post> CreateAsync(Post post);
        
        // READ - Tekil post getirme operasyonları
        Task<Post?> GetByIdAsync(int id);
        Task<Post?> GetBySlugAsync(string slug);
        
        // READ - İlişkili data ile getirme (Navigation Properties)
        Task<Post?> GetWithDetailsAsync(int id); // Post + User + Category
        Task<Post?> GetWithAuthorAsync(int id);  // Post + User
        Task<Post?> GetWithCategoryAsync(int id); // Post + Category
        
        // READ - Çoklu post getirme (Filtering & Pagination)
        Task<IEnumerable<Post>> GetAllAsync();
        Task<IEnumerable<Post>> GetPublishedAsync();
        Task<IEnumerable<Post>> GetDraftsByUserAsync(int userId);
        Task<IEnumerable<Post>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Post>> GetByAuthorAsync(int userId);
        
        // READ - Pagination için (Performance Critical)
        Task<IEnumerable<Post>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Post>> GetPublishedPagedAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<int> GetPublishedCountAsync();
        
        // READ - Search Operations (Full-text search)
        Task<IEnumerable<Post>> SearchAsync(string searchTerm);
        Task<IEnumerable<Post>> SearchInTitleAsync(string searchTerm);
        Task<IEnumerable<Post>> SearchInContentAsync(string searchTerm);
        
        // READ - Date-based filtering (Blog functionality)
        Task<IEnumerable<Post>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Post>> GetRecentAsync(int count);
        Task<IEnumerable<Post>> GetPopularAsync(int count); // Future: view count based
        
        // UPDATE - Post güncelleme
        Task<Post> UpdateAsync(Post post);
        Task<bool> PublishAsync(int postId); // Business logic: Draft → Published
        Task<bool> UnpublishAsync(int postId); // Business logic: Published → Draft
        
        // DELETE - Post silme
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
        
        // UTILITY - Business Rules & Validations
        Task<bool> ExistsAsync(int id);
        Task<bool> SlugExistsAsync(string slug);
        Task<bool> IsAuthorAsync(int postId, int userId); // Authorization check
        Task<bool> IsPublishedAsync(int postId);
        
        // STATISTICS - Dashboard için
        Task<int> GetPostCountByUserAsync(int userId);
        Task<int> GetPostCountByCategoryAsync(int categoryId);
        Task<Dictionary<string, int>> GetPostCountByMonthAsync(int year);
    }
} 