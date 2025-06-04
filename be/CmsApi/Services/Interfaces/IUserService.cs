using CmsApi.DTOs.Requests;
using CmsApi.DTOs.Responses;

namespace CmsApi.Services.Interfaces
{
    /// <summary>
    /// User Service Interface - Kullanıcı işlemlerinin "menüsü"
    /// 
    /// BASIT AÇIKLAMA:
    /// Bu interface, kullanıcılarla ilgili yapabileceğimiz işlemlerin "menüsü" gibidir.
    /// Tıpkı bir restoranda "bugün ne yemekler var?" diye sorduğumuzda aldığımız menü gibi.
    /// Burada "kullanıcılarla ne işlemler yapabiliriz?" sorusunun cevabı var.
    /// 
    /// NEDEN INTERFACE KULLANIYORUZ?
    /// Interface = Sözleşme gibidir. 
    /// "Bu işlemleri yapacağım" diye söz veriyoruz, ama "nasıl yapacağım"ı sonra söylüyoruz.
    /// Böylece kodumuz esnek olur, testleri kolay yazarız.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Yeni kullanıcı kaydı - "Müşteri kayıt olmak istiyor"
        /// 
        /// BASIT AÇIKLAMA:
        /// Birisi sitemize üye olmak istediğinde bu metodu çağırırız.
        /// - Şifreyi güvenli hale getirir (hash'ler)
        /// - Email'in daha önce kullanılmadığını kontrol eder
        /// - Kullanıcı adının benzersiz olduğunu kontrol eder
        /// - Başarılı olursa kullanıcı bilgilerini döndürür
        /// </summary>
        /// <param name="createDto">Kayıt için gerekli bilgiler (ad, email, şifre vs.)</param>
        /// <returns>Oluşturulan kullanıcının güvenli bilgileri</returns>
        Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto createDto);

        /// <summary>
        /// Kullanıcı girişi - "Bu kişi gerçekten kimliğini kanıtlayabiliyor mu?"
        /// 
        /// BASIT AÇIKLAMA:
        /// Birisi "Ben X kullanıcısıyım" dediğinde, gerçekten o mu diye kontrol ederiz.
        /// - Kullanıcı adı/email ile veritabanında arar
        /// - Girilen şifre ile kayıtlı şifreyi karşılaştırır
        /// - Doğruysa, "giriş anahtarı" (token) verir
        /// - Yanlışsa, hata mesajı döndürür
        /// </summary>
        /// <param name="loginDto">Giriş bilgileri (kullanıcı adı/email + şifre)</param>
        /// <returns>Giriş sonucu (başarılı/başarısız + token)</returns>
        Task<AuthResponseDto> LoginAsync(UserLoginRequestDto loginDto);

        /// <summary>
        /// Kullanıcı bilgilerini getir - "Bu ID'li kullanıcının bilgilerini ver"
        /// 
        /// BASIT AÇIKLAMA:
        /// Belirli bir kullanıcının bilgilerini görmek istediğimizde kullanırız.
        /// Örneğin: Profil sayfası, yazarın bilgileri gösterme vs.
        /// Hassas bilgileri (şifre) döndürmez, sadece güvenli bilgileri verir.
        /// </summary>
        /// <param name="userId">Hangi kullanıcının bilgilerini istediğimiz</param>
        /// <returns>Kullanıcının güvenli bilgileri</returns>
        Task<UserResponseDto?> GetUserByIdAsync(int userId);

        /// <summary>
        /// Kullanıcı adı ile kullanıcı bul - "Bu kullanıcı adındaki kişi var mı?"
        /// 
        /// BASIT AÇIKLAMA:
        /// Kullanıcı adını bildiğimizde, o kişinin bilgilerini getirmek için kullanırız.
        /// Örneğin: "@johndoe" şeklinde kullanıcı profili arama
        /// </summary>
        /// <param name="username">Aradığımız kullanıcı adı</param>
        /// <returns>Bulunan kullanıcının bilgileri (yoksa null)</returns>
        Task<UserResponseDto?> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Email ile kullanıcı bul - "Bu email adresli kullanıcı var mı?"
        /// 
        /// BASIT AÇIKLAMA:
        /// Email adresini bildiğimizde kullanıcıyı bulmak için kullanırız.
        /// Genellikle şifre sıfırlama, email doğrulama gibi durumlarda kullanılır.
        /// </summary>
        /// <param name="email">Aradığımız email adresi</param>
        /// <returns>Bulunan kullanıcının bilgileri (yoksa null)</returns>
        Task<UserResponseDto?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Tüm kullanıcıları listele - "Sistemdeki tüm kullanıcıları göster"
        /// 
        /// BASIT AÇIKLAMA:
        /// Admin panelinde kullanıcı listesini göstermek için kullanırız.
        /// Sayfalama özelliği var: 10'ar 10'ar göster gibi.
        /// Çok kullanıcı varsa hepsini birden getirmez, yavaş olur.
        /// </summary>
        /// <param name="page">Hangi sayfa (1, 2, 3...)</param>
        /// <param name="pageSize">Sayfada kaç kullanıcı (10, 20, 50...)</param>
        /// <returns>Kullanıcı listesi + toplam sayı bilgisi</returns>
        Task<(List<UserResponseDto> users, int totalCount)> GetUsersAsync(int page = 1, int pageSize = 20);

        /// <summary>
        /// Kullanıcı bilgilerini güncelle - "Bu kullanıcının bilgilerini değiştir"
        /// 
        /// BASIT AÇIKLAMA:
        /// Kullanıcı profil bilgilerini değiştirmek istediğinde kullanırız.
        /// Örneğin: Ad soyad değiştirme, email güncelleme vs.
        /// Şifre değişikliği ayrı bir metot ile yapılır (güvenlik için).
        /// </summary>
        /// <param name="userId">Hangi kullanıcının bilgilerini güncelleyeceğiz</param>
        /// <param name="updateDto">Yeni bilgiler</param>
        /// <returns>Güncellenmiş kullanıcı bilgileri</returns>
        Task<UserResponseDto> UpdateUserAsync(int userId, UserUpdateRequestDto updateDto);

        /// <summary>
        /// Şifre değiştir - "Bu kullanıcının şifresini güvenli şekilde değiştir"
        /// 
        /// BASIT AÇIKLAMA:
        /// Şifre değişikliği hassas bir işlem olduğu için ayrı metot.
        /// - Eski şifrenin doğru olduğunu kontrol eder
        /// - Yeni şifreyi güvenli hale getirir (hash'ler)
        /// - Veritabanında günceller
        /// </summary>
        /// <param name="userId">Hangi kullanıcının şifresi</param>
        /// <param name="changePasswordDto">Eski şifre + yeni şifre</param>
        /// <returns>İşlem başarılı/başarısız bilgisi</returns>
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequestDto changePasswordDto);

        /// <summary>
        /// Kullanıcıyı aktif/pasif yap - "Bu kullanıcının hesabını aç/kapat"
        /// 
        /// BASIT AÇIKLAMA:
        /// Kullanıcının hesabını geçici olarak kapatmak/açmak için kullanırız.
        /// Pasif kullanıcılar giriş yapamaz, ama bilgileri silinmez.
        /// Örneğin: Kural ihlali yapan kullanıcıyı geçici askıya alma.
        /// </summary>
        /// <param name="userId">Hangi kullanıcı</param>
        /// <param name="isActive">true = aktif, false = pasif</param>
        /// <returns>İşlem başarılı/başarısız bilgisi</returns>
        Task<bool> SetUserActiveStatusAsync(int userId, bool isActive);

        /// <summary>
        /// Kullanıcıyı tamamen sil - "Bu kullanıcıyı sistemden tamamen kaldır"
        /// 
        /// BASIT AÇIKLAMA:
        /// Kullanıcının tüm bilgilerini sistemden siler.
        /// ÇOK DİKKATLİ kullanılmalı! Geri dönüşü yoktur.
        /// Genellikle GDPR (veri koruma) talebi geldiğinde kullanılır.
        /// </summary>
        /// <param name="userId">Hangi kullanıcı silinecek</param>
        /// <returns>İşlem başarılı/başarısız bilgisi</returns>
        Task<bool> DeleteUserAsync(int userId);

        /// <summary>
        /// Kullanıcı var mı kontrol et - "Bu email/kullanıcı adı daha önce alınmış mı?"
        /// 
        /// BASIT AÇIKLAMA:
        /// Yeni kayıt sırasında, email veya kullanıcı adının benzersiz olduğunu kontrol ederiz.
        /// Eğer daha önce alınmışsa, kullanıcıya "Bu email zaten kullanımda" deriz.
        /// </summary>
        /// <param name="email">Kontrol edilecek email</param>
        /// <param name="username">Kontrol edilecek kullanıcı adı</param>
        /// <returns>true = zaten var, false = kullanılabilir</returns>
        Task<bool> IsUserExistsAsync(string? email = null, string? username = null);
    }

    /// <summary>
    /// Şifre değişikliği için gerekli bilgiler
    /// 
    /// BASIT AÇIKLAMA:
    /// Şifre değiştirirken hem eski şifreyi hem yeni şifreyi istiyoruz.
    /// Bu güvenlik için çok önemli: "Ben gerçekten bu hesabın sahibiyim" kanıtı.
    /// </summary>
    public class ChangePasswordRequestDto
    {
        /// <summary>Mevcut şifre - "Ben gerçekten bu hesabın sahibiyim" kanıtı</summary>
        public string CurrentPassword { get; set; } = string.Empty;
        
        /// <summary>Yeni şifre - "Artık bu şifreyi kullanmak istiyorum"</summary>
        public string NewPassword { get; set; } = string.Empty;
        
        /// <summary>Yeni şifre tekrar - "Şifremi doğru yazdığımdan emin olmak için"</summary>
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// Kullanıcı güncelleme için gerekli bilgiler
    /// 
    /// BASIT AÇIKLAMA:
    /// Profil güncellerken hangi bilgilerin değişebileceğini tanımlar.
    /// Şifre burada yok, çünkü şifre değişikliği ayrı ve daha güvenli bir işlem.
    /// </summary>
    public class UserUpdateRequestDto
    {
        /// <summary>Yeni ad (isteğe bağlı)</summary>
        public string? FirstName { get; set; }
        
        /// <summary>Yeni soyad (isteğe bağlı)</summary>
        public string? LastName { get; set; }
        
        /// <summary>Yeni email (benzersiz olmalı)</summary>
        public string? Email { get; set; }
    }
} 