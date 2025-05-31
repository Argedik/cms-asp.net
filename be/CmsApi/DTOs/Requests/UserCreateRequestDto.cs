using System.ComponentModel.DataAnnotations;

namespace CmsApi.DTOs.Requests
{
    /// <summary>
    /// User creation request DTO
    /// SRP: Sadece user creation input data sorumluluğu
    /// Validation attributes ile business rules enforced
    /// Domain model'den ayrı olarak API contract'ı tanımlar
    /// </summary>
    public class UserCreateRequestDto
    {
        /// <summary>
        /// Unique username for the user
        /// Business rules: 3-50 characters, alphanumeric + underscore
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// User's email address - used for login and notifications
        /// Business rules: Valid email format, unique in system
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's password - will be hashed before storage
        /// Business rules: Strong password requirements
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Password confirmation for client-side validation
        /// Business rule: Must match Password field
        /// </summary>
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// User's first name (optional)
        /// </summary>
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string? FirstName { get; set; }

        /// <summary>
        /// User's last name (optional)
        /// </summary>
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string? LastName { get; set; }

        /// <summary>
        /// User role assignment (Admin only)
        /// Default: "User", Options: "User", "Editor", "Admin"
        /// </summary>
        public string Role { get; set; } = "User";
    }
} 