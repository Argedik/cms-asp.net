# PostgreSQL Kurulumu İçin Tüm Adımlar

1. Gerekli Paketlerin ve Sistem Güncellemelerinin Yapılması
  Sistemin güncel ve sağlıklı çalışması için öncelikle sistem paketlerinin güncellenmesi gerekir.

2. PostgreSQL Paket Deposu Eklenmesi (Opsiyonel, en güncel sürüm için önerilir)
  Varsayılan depodaki sürüm eski olabilir. En güncel kararlı sürümü almak için resmi PostgreSQL deposu eklenir.
  
3. PostgreSQL Kurulumunun Yapılması
  PostgreSQL ana paketi ve ek araçlar kurulur.
  
4. PostgreSQL Servisinin Başlatılması ve Otomatik Başlatmaya Alınması
  Servisin çalıştığından ve her yeniden başlatmada otomatik başlayacağından emin olunur.

5. Kurulumun Doğrulanması
  Versiyon kontrolü ve servis durumu kontrol edilir.

6. postgres Kullanıcısı ile Veritabanı Yöneticisi Olarak Giriş Yapılması
  Varsayılan olarak oluşturulan postgres kullanıcısı ile giriş yapılır.

7. Veritabanı ve Kullanıcı Oluşturulması (İlk Test)
  Kendi test veritabanını ve kullanıcıyı oluşturup temel komutlar denenir.

8. Uzak Bağlantı (Remote Access) ve Güvenlik Ayarlarının Yapılması (İsteğe Bağlı)
  İstersen PostgreSQL’e başka makinelerden de erişmek için ek ayarlar yapılır.

9. Ekstra: PostgreSQL için Popüler Eklenti/Arayüzlerin Kurulması
  pgAdmin, DBeaver, PostGIS gibi ihtiyaç duyulursa ek araçlar kurulur.

---

## PostgreSQL kurulum sonrası
