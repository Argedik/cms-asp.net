using CmsApi.Models;

namespace CmsApi.Services.Interfaces
{
    /// <summary>
    /// Category business logic operations interface
    /// SRP: Category ile ilgili business logic sorumluluğu
    /// DIP: Concrete implementation'a bağımlı değil
    /// Business rules: Slug generation, hierarchy validation, deletion rules
    /// </summary>
    public interface ICategoryService
    {
        // CATEGORY CRUD - Business Logic Operations
        Task<(bool Success, Category? Category, string Message)> CreateCategoryAsync(string name, string description);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryByNameAsync(string name);
        Task<Category?> GetCategoryBySlugAsync(string slug);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        
        // CATEGORY WITH RELATIONSHIPS - Business Logic
        Task<Category?> GetCategoryWithPostsAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesWithPostCountAsync();
        Task<IEnumerable<Category>> GetPopularCategoriesAsync(int limit); // Post count'a göre
        
        // CATEGORY UPDATE - Complex Business Logic
        Task<(bool Success, Category? Category, string Message)> UpdateCategoryAsync(int id, string? name, string? description);
        Task<(bool Success, string Message)> ActivateCategoryAsync(int id, int actionUserId);
        Task<(bool Success, string Message)> DeactivateCategoryAsync(int id, int actionUserId);
        
        // CATEGORY DELETION - Business Rules
        Task<(bool Success, string Message)> DeleteCategoryAsync(int id, int actionUserId);
        Task<(bool Success, string Message)> SafeDeleteCategoryAsync(int id, int? moveToCategoyId, int actionUserId);
        
        // SLUG MANAGEMENT - Business Logic
        Task<string> GenerateSlugAsync(string name);
        Task<string> GenerateUniqueSlugAsync(string name, int? excludeId = null);
        Task<(bool IsValid, string Message)> ValidateSlugAsync(string slug, int? excludeId = null);
        
        // VALIDATION & BUSINESS RULES
        Task<(bool IsValid, string Message)> ValidateCategoryDataAsync(string name, int? excludeId = null);
        Task<bool> IsCategoryNameAvailableAsync(string name, int? excludeId = null);
        Task<bool> CanDeleteCategoryAsync(int id); // Has posts kontrolü
        
        // POST MANAGEMENT WITHIN CATEGORY - Business Operations
        Task<IEnumerable<Post>> GetCategoryPostsAsync(int categoryId, bool includeInactive = false);
        Task<int> GetCategoryPostCountAsync(int categoryId);
        Task<(bool Success, string Message)> MoveCategoryPostsAsync(int fromCategoryId, int toCategoryId, int actionUserId);
        
        // CATEGORY STATISTICS - Business Analytics
        Task<int> GetTotalCategoryCountAsync();
        Task<int> GetActiveCategoryCountAsync();
        Task<Dictionary<string, int>> GetCategoryUsageStatsAsync(); // Category name -> post count
        Task<IEnumerable<Category>> GetUnusedCategoriesAsync(); // Post'u olmayan kategoriler
        
        // BULK OPERATIONS - Advanced Business Logic
        Task<(int Success, int Failed, string Message)> BulkActivateCategoriesAsync(int[] categoryIds, int actionUserId);
        Task<(int Success, int Failed, string Message)> BulkDeactivateCategoriesAsync(int[] categoryIds, int actionUserId);
        Task<(int Success, int Failed, string Message)> BulkDeleteCategoriesAsync(int[] categoryIds, int actionUserId);
        
        // SEARCH & FILTERING - Business Logic
        Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm);
        Task<IEnumerable<Category>> GetCategoriesByPostCountRangeAsync(int minPosts, int maxPosts);
        Task<IEnumerable<Category>> GetRecentlyUsedCategoriesAsync(int limit); // Son post eklenen kategoriler
    }
} 