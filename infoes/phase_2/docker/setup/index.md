# Docker kurulum kontrolü

1. docker --version / which docker / docker compose version

---

## Docker kurulum

- Güncellemeleri yapıyoruz;
[***kaynak***](´https://docs.docker.com/engine/install/ubuntu/#install-using-the-repository`)
  - sudo apt update
  - sudo apt upgrade -y

- Genelde zaten sistemde yüklü olan aşağıdaki paketlerin kontrolleri;
  - dpkg -l | grep ca-certificates
  - dpkg -l | grep curl
  - dpkg -l | grep gnupg
  - dpkg -l | grep lsb-release

## Docker'da tüm konutları varsayılan olan sudo ile başlatma

- sudo usermod -aG docker $USER
