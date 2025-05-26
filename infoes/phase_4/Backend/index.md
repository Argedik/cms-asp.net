
# Veritabanı Şemalarını Oluşturduk

users, templates, projects, project_settings, project_sections, project_elements, social_media_accounts, contact_forms tablolarını içeren bir SQL migration dosyası oluşturduk.

# Rust Modelleri Tanımladık

User ve Project modelleri gibi veritabanı tablolarına karşılık gelen yapılar tanımladık.
DTO (Data Transfer Object) yapıları ekledik.

# Veritabanı Bağlantısı İçin Gerekli Altyapıyı Hazırladık

deadpool-postgres kullanarak bağlantı havuzu oluşturduk.
Ana CMS veritabanı ve her proje için ayrı bağlantı havuzu desteği ekledik.

# Migration Sistemini Kurduk

refinery kütüphanesi ile veritabanı migrasyonlarını yönetecek sistemi ekledik.
Proje için otomatik veritabanı oluşturma ve silme işlevlerini ekledik.
