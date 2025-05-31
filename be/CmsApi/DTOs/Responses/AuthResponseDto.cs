namespace CmsApi.DTOs.Responses
{
    /// <summary>
    /// Authentication response DTO
    /// SRP: Sadece authentication result data sorumluluğu
    /// Security: Token management and user context
    /// Business logic: Successful/failed authentication handling
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// Authentication success status
        /// Business rule: Determines client-side behavior
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Authentication result message
        /// UX: User-friendly feedback for login attempts
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// JWT access token (if authentication successful)
        /// Security: Used for API authorization
        /// Null if authentication failed
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Token type (always "Bearer" for JWT)
        /// Standard: OAuth 2.0 token type specification
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Token expiration time in seconds
        /// Client behavior: When to refresh token
        /// Business rule: Configurable via appsettings
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Token expiration timestamp
        /// UX: Display session expiry to user
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Authenticated user information
        /// Context: User details for client application
        /// Null if authentication failed
        /// </summary>
        public UserResponseDto? User { get; set; }

        /// <summary>
        /// Refresh token for token renewal (optional)
        /// Security: Long-lived token for getting new access tokens
        /// Future enhancement: Token refresh mechanism
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// User permissions/roles array
        /// Authorization: Client-side permission checks
        /// Performance: Avoid repeated authorization calls
        /// </summary>
        public string[] Permissions { get; set; } = Array.Empty<string>();
    }
} 