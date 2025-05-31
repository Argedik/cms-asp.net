namespace CmsApi.DTOs.Responses
{
    /// <summary>
    /// User response DTO for API output
    /// SRP: Sadece user data presentation sorumluluğu
    /// Security: Sensitive data (password, tokens) excluded
    /// Clean API contract: Domain model'den bağımsız
    /// </summary>
    public class UserResponseDto
    {
        /// <summary>
        /// User's unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User's unique username
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// User's email address
        /// Security note: Only shown to user themselves or admins
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's first name (if provided)
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// User's last name (if provided)
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// User's full name (computed property)
        /// Business logic: Combines FirstName and LastName
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();

        /// <summary>
        /// User's role in the system
        /// Authorization: Determines user permissions
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Whether the user account is active
        /// Business rule: Inactive users cannot login
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// When the user account was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When the user last updated their profile
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// User's last login time (nullable)
        /// Privacy: Only shown to user themselves or admins
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Number of posts authored by this user
        /// Computed property: Aggregated from Posts table
        /// </summary>
        public int PostCount { get; set; }
    }
} 