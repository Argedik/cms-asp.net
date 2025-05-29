using CmsApi.Models;

namespace CmsApi.Services.Interfaces
{
    /// <summary>
    /// User business logic operations interface
    /// SRP: User ile ilgili business logic sorumluluğu
    /// DIP: Service implementation'a bağımlı değil, sadece contract
    /// Business Logic vs Data Access ayrımı yapılıyor
    /// </summary>
    public interface IUserService
    {
        // AUTHENTICATION & AUTHORIZATION - Business Logic
        Task<(bool Success, string Token, string Message)> LoginAsync(string username, string password);
        Task<(bool Success, User? User, string Message)> RegisterAsync(string username, string email, string password, string role = "User");
        Task<bool> LogoutAsync(string token); // Token blacklisting için
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        
        // USER MANAGEMENT - Business Operations
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetActiveUsersAsync();
        
        // USER PROFILE MANAGEMENT - Complex Business Logic
        Task<(bool Success, User? User, string Message)> UpdateProfileAsync(int userId, string? username, string? email, string? firstName, string? lastName);
        Task<(bool Success, string Message)> DeactivateUserAsync(int userId, int actionUserId); // Audit için actionUserId
        Task<(bool Success, string Message)> ActivateUserAsync(int userId, int actionUserId);
        
        // ROLE & PERMISSION MANAGEMENT - Authorization Logic
        Task<(bool Success, string Message)> AssignRoleAsync(int userId, string role, int adminUserId);
        Task<IEnumerable<string>> GetUserRolesAsync(int userId);
        Task<bool> HasPermissionAsync(int userId, string permission);
        
        // VALIDATION & BUSINESS RULES - Business Logic
        Task<(bool IsValid, string Message)> ValidateUserDataAsync(string username, string email, int? excludeUserId = null);
        Task<bool> IsUsernameAvailableAsync(string username, int? excludeUserId = null);
        Task<bool> IsEmailAvailableAsync(string email, int? excludeUserId = null);
        Task<(bool IsValid, string Message)> ValidatePasswordStrengthAsync(string password);
        
        // USER STATISTICS & ANALYTICS - Business Calculations
        Task<int> GetTotalUserCountAsync();
        Task<int> GetActiveUserCountAsync();
        Task<int> GetUserRegistrationCountByMonthAsync(int year, int month);
        Task<Dictionary<string, int>> GetUserCountByRoleAsync();
        
        // USER ACTIVITY - Complex Business Operations
        Task<IEnumerable<Post>> GetUserPostsAsync(int userId, bool includeInactive = false);
        Task<int> GetUserPostCountAsync(int userId);
        Task<DateTime?> GetLastLoginTimeAsync(int userId);
        Task<(bool Success, string Message)> UpdateLastLoginAsync(int userId);
        
        // BULK OPERATIONS - Advanced Business Logic
        Task<(int Success, int Failed, string Message)> BulkDeactivateUsersAsync(int[] userIds, int actionUserId);
        Task<(int Success, int Failed, string Message)> BulkDeleteUsersAsync(int[] userIds, int actionUserId);
        
        // EMAIL & NOTIFICATION - Business Process
        Task<(bool Success, string Message)> SendPasswordResetEmailAsync(string email);
        Task<(bool Success, string Message)> ResetPasswordAsync(string email, string resetToken, string newPassword);
        Task<(bool Success, string Message)> SendEmailVerificationAsync(int userId);
        Task<(bool Success, string Message)> VerifyEmailAsync(int userId, string verificationToken);
    }
} 