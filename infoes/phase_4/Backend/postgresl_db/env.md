.env dosyası oluşturuluyor
# Veritabanı Yapılandırması
DB_HOST=localhost (Varsayılan veritabanı IP adresi. 127.0.0.1 IP adresine karşılık gelir.)
DB_PORT=5432 (Varsayılan veri tabanı portu, aşağıdaki gibi öğrenilebilir.)
    ➜  cms-rust git:(master) ✗ sudo cat /etc/postgresql/17/main/postgresql.conf | grep port
      port = 5432				# (change requires restart)
      #ssl_passphrase_command_supports_reload = off
                # supported by the operating system:
                # supported by the operating system:
                # supported by the operating system:
                #   %r = remote host and port
      #icu_validation_level = warning		# report ICU locale validation

DB_USER=postgres (Varsayılan kullanıcı adı ve tüm yetkilere sahip)
➜  cms-rust git:(master) ✗ sudo -u postgres psql -c "\du"
                             List of roles
 Role name |                         Attributes                         
-----------+------------------------------------------------------------
 postgres  | Superuser, Create role, Create DB, Replication, Bypass RLS
DB_PASSWORD=password (Yeni şifre oluşturmak için aşağıdaki kodda "yeni_şifre" yazan yeri değiştirebiliriz.)
sudo -u postgres psql -c "ALTER USER postgres WITH PASSWORD 'yeni_şifre';"

DB_NAME=cms (sudo -u postgres psql üzerinden \l ile öğrenilen veri tabanı)

DB_MAX_CONNECTIONS=5 (Aynı anda açık olan maksimum veri tabanı bağlantı sayısı)
DB_CONNECTION_TIMEOUT=30 (30 saniye bağlantı kurulamazsa bağlantı denemesi iptal edilir ya da hata döndürülür.)

# Sunucu Yapılandırması
SERVER_HOST=127.0.0.1 (ya da  0.0.0.0 ile modeme bağlı olan tüm cihazlardan erişmek için 
➜  cms-rust git:(master) ✗ ip addr show | grep inet
    inet 127.0.0.1/8 scope host lo
    inet6 ::1/128 scope host noprefixroute 
    inet 172.17.0.1/16 brd 172.17.255.255 scope global docker0
    inet 192.168.1.111/24 brd 192.168.1.255 scope global dynamic noprefixroute wlx986ee8259e76
    inet6 fe80::82a7:ee82:7ad4:69c5/64 scope link noprefixroute 

    bu bilgisayarın ip adresi: 192.168.1.111
)

SERVER_PORT=8080

# JWT Yapılandırması
JWT_SECRET=1234567890abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz
(Aşağıdaki gibi değiştirilebilir.
openssl rand -base64 32
tONIzqw4fFL4HVGS805+HZAhCSAvBtYwgcpZLInRgSc=
)

JWT_EXPIRATION=86400 (Tokenin çalışma süresi)

# Log Seviyesi
LOG_LEVEL=info (Uygulama çalışırken nasıl loglanması gerektiğini belirtiyor.)