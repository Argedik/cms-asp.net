using CmsApi.Models;

namespace CmsApi.Repositories.Interfaces
{
    /// <summary>
    /// User entity için data access operasyonlarını tanımlayan interface
    /// DIP (Dependency Inversion Principle) gereği concrete class yerine interface kullanıyoruz
    /// </summary>
    public interface IUserRepository
    {
        // CREATE - Yeni user oluşturma
        Task<User> CreateAsync(User user);
        
        // READ - Tekil user getirme operasyonları
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        
        // READ - Çoklu user getirme operasyonları
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetActiveUsersAsync();
        
        // UPDATE - User güncelleme
        Task<User> UpdateAsync(User user);
        
        // DELETE - User silme (Soft delete için IsActive=false yapacağız)
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
        
        // UTILITY - Yardımcı metodlar
        Task<bool> ExistsAsync(int id);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
} 