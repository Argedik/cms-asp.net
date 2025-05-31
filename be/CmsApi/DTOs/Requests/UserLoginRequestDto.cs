using System.ComponentModel.DataAnnotations;

namespace CmsApi.DTOs.Requests
{
    /// <summary>
    /// User login request DTO
    /// SRP: Sadece authentication input data sorumluluğu
    /// Minimal validation for login process
    /// Security: No sensitive data exposure
    /// </summary>
    public class UserLoginRequestDto
    {
        /// <summary>
        /// Username or email for authentication
        /// Business rule: Support both username and email login
        /// </summary>
        [Required(ErrorMessage = "Username or email is required")]
        [StringLength(200, ErrorMessage = "Username/email cannot exceed 200 characters")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        /// <summary>
        /// User's password for authentication
        /// Security: Sent over HTTPS, never logged
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Remember me option for extended session
        /// Default: false (security-first approach)
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }
} 