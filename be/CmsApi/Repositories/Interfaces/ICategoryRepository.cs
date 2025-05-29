using CmsApi.Models;

namespace CmsApi.Repositories.Interfaces
{
    /// <summary>
    /// Category entity için data access operasyonlarını tanımlayan interface
    /// SRP: Sadece Category ile ilgili data access sorumluluğu
    /// DIP: Abstract interface, concrete implementation'a bağımlı değil
    /// </summary>
    public interface ICategoryRepository
    {
        // CREATE - Yeni category oluşturma
        Task<Category> CreateAsync(Category category);
        
        // READ - Tekil category getirme operasyonları
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetByNameAsync(string name);
        Task<Category?> GetBySlugAsync(string slug);
        
        // READ - İlişkili data ile getirme (Navigation Properties)
        Task<Category?> GetWithPostsAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesWithPostCountAsync();
        
        // READ - Çoklu category getirme operasyonları
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetActiveAsync();
        
        // UPDATE - Category güncelleme
        Task<Category> UpdateAsync(Category category);
        
        // DELETE - Category silme
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
        
        // UTILITY - Yardımcı metodlar (Business Rules)
        Task<bool> ExistsAsync(int id);
        Task<bool> NameExistsAsync(string name);
        Task<bool> SlugExistsAsync(string slug);
        Task<bool> HasPostsAsync(int categoryId);
        Task<int> GetPostCountAsync(int categoryId);
    }
} 