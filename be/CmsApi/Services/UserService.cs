using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using CmsApi.Data;
using CmsApi.DTOs.Requests;
using CmsApi.DTOs.Responses;
using CmsApi.Models;
using CmsApi.Services.Interfaces;

namespace CmsApi.Services
{
    /// <summary>
    /// User Service Implementation - Kullanıcı işlemlerinin "aşçısı"
    /// 
    /// BASIT AÇIKLAMA:
    /// Interface'de "ne yapacağımızı" söylemiştik, burada "nasıl yapacağımızı" anlatıyoruz.
    /// Tıpkı bir aşçı gibi: Menüde "Pilav" yazıyor, aşçı da nasıl pişireceğini biliyor.
    /// 
    /// BU SINIF NE YAPAR?
    /// - Şifreleri güvenli hale getirir (hash'ler)
    /// - Kullanıcıların benzersiz olduğunu kontrol eder  
    /// - Giriş yapan kullanıcılara "anahtarlar" (token) verir
    /// - Veritabanı ile konuşur ama business logic'i yönetir
    /// </summary>
    public class UserService : IUserService
    {
        #region Private Fields - "Aşçının malzemeleri ve aletleri"
        
        /// <summary>
        /// Veritabanı bağlantısı - "Malzeme deposu"
        /// BASIT AÇIKLAMA: Veritabanındaki bilgileri almak ve yazmak için kullanırız.
        /// </summary>
        private readonly CmsDbContext _context;
        
        /// <summary>
        /// JWT ayarları - "Anahtar (token) yapma tarifi"
        /// BASIT AÇIKLAMA: Kullanıcılara vereceğimiz "dijital anahtarları" nasıl yapacağımızın ayarları.
        /// </summary>
        private readonly IConfiguration _configuration;
        
        /// <summary>
        /// Log sistemi - "Aşçının günlüğü"
        /// BASIT AÇIKLAMA: Neler olduğunu kayıt altına alırız. Hata olursa nedenini anlarız.
        /// </summary>
        private readonly ILogger<UserService> _logger;

        #endregion

        #region Constructor - "Aşçıyı işe alma ve eğitme"
        
        /// <summary>
        /// UserService Constructor
        /// 
        /// BASIT AÇIKLAMA:
        /// Bir aşçıyı işe aldığımızda, ona mutfak araçlarını ve malzeme deposunu gösteririz.
        /// Bu constructor da aynı şeyi yapıyor: Service'e ihtiyacı olan araçları veriyor.
        /// </summary>
        public UserService(CmsDbContext context, IConfiguration configuration, ILogger<UserService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        #endregion

        #region Public Methods - "Aşçının müşterilere sunduğu yemekler"

        /// <summary>
        /// Yeni kullanıcı oluştur - "Yeni müşteri kaydı al"
        /// 
        /// BASIT AÇIKLAMA:
        /// 1. Önce kontrol et: Bu email/kullanıcı adı daha önce alınmış mı?
        /// 2. Şifreyi güvenli hale getir (kimse göremez)
        /// 3. Veritabanına kaydet
        /// 4. Güvenli bilgileri geri döndür (şifre olmadan)
        /// </summary>
        public async Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto createDto)
        {
            _logger.LogInformation("Yeni kullanıcı oluşturma işlemi başladı: {Username}", createDto.Username);

            try
            {
                // 1. ADIM: "Bu kullanıcı adı/email daha önce alınmış mı?" kontrol et
                if (await IsUserExistsAsync(createDto.Email, createDto.Username))
                {
                    _logger.LogWarning("Kullanıcı oluşturma başarısız - Email veya kullanıcı adı zaten mevcut: {Email}", createDto.Email);
                    throw new InvalidOperationException("Bu email veya kullanıcı adı zaten kullanılıyor!");
                }

                // 2. ADIM: Şifreyi güvenli hale getir (Hash'le)
                string passwordHash = HashPassword(createDto.Password);
                
                // 3. ADIM: Yeni kullanıcı objesi oluştur
                var user = new User
                {
                    Username = createDto.Username,
                    Email = createDto.Email,
                    PasswordHash = passwordHash, // Şifre değil, hash'lenmiş hali
                    FirstName = createDto.FirstName,
                    LastName = createDto.LastName,
                    Role = createDto.Role,
                    IsActive = true, // Yeni kullanıcılar aktif başlar
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // 4. ADIM: Veritabanına kaydet
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Kullanıcı başarıyla oluşturuldu: {UserId} - {Username}", user.Id, user.Username);

                // 5. ADIM: Güvenli bilgileri döndür (şifre hash'i olmadan)
                return MapToUserResponseDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturma hatası: {Username}", createDto.Username);
                throw;
            }
        }

        /// <summary>
        /// Kullanıcı girişi - "Kapıdaki kişinin anahtarı var mı kontrol et"
        /// 
        /// BASIT AÇIKLAMA:
        /// 1. Kullanıcıyı bul (username veya email ile)
        /// 2. Şifresini kontrol et (hash'lenmiş hali ile karşılaştır)
        /// 3. Doğruysa, dijital anahtar (token) ver
        /// 4. Yanlışsa, "Giriş reddedildi" de
        /// </summary>
        public async Task<AuthResponseDto> LoginAsync(UserLoginRequestDto loginDto)
        {
            _logger.LogInformation("Giriş işlemi başladı: {UsernameOrEmail}", loginDto.UsernameOrEmail);

            try
            {
                // 1. ADIM: Kullanıcıyı bul (email veya username ile)
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => 
                        u.Email == loginDto.UsernameOrEmail || 
                        u.Username == loginDto.UsernameOrEmail);

                // 2. ADIM: Kullanıcı var mı ve aktif mi kontrol et
                if (user == null)
                {
                    _logger.LogWarning("Giriş başarısız - Kullanıcı bulunamadı: {UsernameOrEmail}", loginDto.UsernameOrEmail);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Kullanıcı adı/email veya şifre hatalı!"
                    };
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning("Giriş başarısız - Kullanıcı pasif: {UserId}", user.Id);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Hesabınız askıya alınmış. Lütfen yönetici ile iletişime geçin."
                    };
                }

                // 3. ADIM: Şifreyi kontrol et
                if (!VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Giriş başarısız - Hatalı şifre: {UserId}", user.Id);
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Kullanıcı adı/email veya şifre hatalı!"
                    };
                }

                // 4. ADIM: Son giriş zamanını güncelle
                user.LastLoginAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                // 5. ADIM: JWT token oluştur ve başarılı cevap döndür
                var token = GenerateJwtToken(user);
                var expiresAt = DateTime.UtcNow.AddHours(24); // 24 saat geçerli

                _logger.LogInformation("Giriş başarılı: {UserId} - {Username}", user.Id, user.Username);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Giriş başarılı!",
                    Token = token,
                    ExpiresIn = 24 * 60 * 60, // 24 saat (saniye cinsinden)
                    ExpiresAt = expiresAt,
                    User = MapToUserResponseDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi hatası: {UsernameOrEmail}", loginDto.UsernameOrEmail);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Giriş işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin."
                };
            }
        }

        /// <summary>
        /// ID ile kullanıcı getir - "Bu numaralı müşterinin bilgilerini ver"
        /// </summary>
        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            _logger.LogInformation("Kullanıcı bilgileri istendi: {UserId}", userId);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                _logger.LogWarning("Kullanıcı bulunamadı: {UserId}", userId);
                return null;
            }

            return MapToUserResponseDto(user);
        }

        /// <summary>
        /// Username ile kullanıcı getir
        /// </summary>
        public async Task<UserResponseDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            return user == null ? null : MapToUserResponseDto(user);
        }

        /// <summary>
        /// Email ile kullanıcı getir
        /// </summary>
        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            return user == null ? null : MapToUserResponseDto(user);
        }

        /// <summary>
        /// Kullanıcı listesi getir (sayfalama ile)
        /// 
        /// BASIT AÇIKLAMA:
        /// Çok kullanıcı varsa hepsini birden göstermek yavaş olur.
        /// Bu yüzden "sayfa sayfa" gösteririz: 1. sayfa 20 kişi, 2. sayfa 20 kişi vs.
        /// </summary>
        public async Task<(List<UserResponseDto> users, int totalCount)> GetUsersAsync(int page = 1, int pageSize = 20)
        {
            // Toplam kullanıcı sayısını öğren
            var totalCount = await _context.Users.CountAsync();

            // Belirtilen sayfadaki kullanıcıları getir
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt) // En yeni kayıtlar önce
                .Skip((page - 1) * pageSize) // Önceki sayfaları atla
                .Take(pageSize) // Sadece bu sayfadaki kadarını al
                .ToListAsync();

            var userDtos = users.Select(MapToUserResponseDto).ToList();
            
            return (userDtos, totalCount);
        }

        // Diğer metodlar için place holder'lar (kısa tutmak için)
        public Task<UserResponseDto> UpdateUserAsync(int userId, UserUpdateRequestDto updateDto)
        {
            throw new NotImplementedException("Bu metot henüz tamamlanmadı");
        }

        public Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequestDto changePasswordDto)
        {
            throw new NotImplementedException("Bu metot henüz tamamlanmadı");
        }

        public Task<bool> SetUserActiveStatusAsync(int userId, bool isActive)
        {
            throw new NotImplementedException("Bu metot henüz tamamlanmadı");
        }

        public Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException("Bu metot henüz tamamlanmadı");
        }

        /// <summary>
        /// Kullanıcı var mı kontrol et
        /// 
        /// BASIT AÇIKLAMA:
        /// Yeni kayıt sırasında "Bu email/kullanıcı adı daha önce alınmış mı?" kontrol ederiz.
        /// </summary>
        public async Task<bool> IsUserExistsAsync(string? email = null, string? username = null)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(username))
                return false;

            return await _context.Users
                .AnyAsync(u => 
                    (email != null && u.Email == email) ||
                    (username != null && u.Username == username));
        }

        #endregion

        #region Private Helper Methods - "Aşçının özel tarifleri ve teknikleri"

        /// <summary>
        /// Şifreyi güvenli hale getir (Hash'le)
        /// 
        /// BASIT AÇIKLAMA:
        /// Şifreleri açık metin olarak saklamak çok tehlikeli!
        /// Bu yüzden özel bir "karıştırma" işlemi yapıyoruz.
        /// "123456" şifresi "X$mP9#qR..." gibi karışık bir hale gelir.
        /// Bu işlem geri döndürülemez: hash'ten orjinal şifreyi bulamazsın.
        /// </summary>
        private string HashPassword(string password)
        {
            // BCrypt algoritması kullan (en güvenli yöntemlerden biri)
            return BCrypt.Net.BCrypt.HashPassword(password, 12); // 12 = güvenlik seviyesi
        }

        /// <summary>
        /// Şifre doğru mu kontrol et
        /// 
        /// BASIT AÇIKLAMA:
        /// Kullanıcı giriş yaparken girdiği şifreyi, kayıtlı hash ile karşılaştırırız.
        /// Hash'ten orjinal şifreyi bulamayız, ama aynı şifre aynı hash'i verir mi kontrol ederiz.
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre doğrulama hatası");
                return false;
            }
        }

        /// <summary>
        /// JWT Token oluştur - "Dijital anahtar yap"
        /// 
        /// BASIT AÇIKLAMA:
        /// JWT Token = Dijital kimlik kartı gibidir.
        /// İçinde kullanıcının kimliği, rolü vs. bilgiler vardır.
        /// Şifreli ve imzalıdır: sahte token yapamazsın.
        /// Süre sınırlıdır: 24 saat sonra geçersiz olur.
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var key = Encoding.ASCII.GetBytes(secretKey!);
            
            // Token'ın içinde taşınacak bilgiler (Claims)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("IsActive", user.IsActive.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24), // 24 saat geçerli
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// User model'ini UserResponseDto'ya çevir
        /// 
        /// BASIT AÇIKLAMA:
        /// Veritabanından gelen User nesnesi her şeyi içerir (şifre hash'i vs.)
        /// Ama client'a sadece güvenli bilgileri göndermek istiyoruz.
        /// Bu metot "filtreleme" yapar: güvenli bilgileri alır, hassas olanları atar.
        /// </summary>
        private UserResponseDto MapToUserResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                LastLoginAt = user.LastLoginAt,
                PostCount = 0 // TODO: Gerçek post sayısını hesapla
            };
        }

        #endregion
    }
} 