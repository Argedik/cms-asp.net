# Postgresql  kurulum kontrolü

- sudo apt update
- sudo apt upgrade -y

---

## Postgresql kurulum

- Güncellemeleri yapıyoruz;
[***kaynak***](´https://docs.docker.com/engine/install/ubuntu/#install-using-the-repository`)
  - sudo apt update
  - sudo apt upgrade -y

Automated repository configuration:

sudo apt install -y postgresql-common
sudo /usr/share/postgresql-common/pgdg/apt.postgresql.org.sh

To manually configure the Apt repository, follow these steps:

sudo apt install curl ca-certificates
sudo install -d /usr/share/postgresql-common/pgdg
sudo curl -o /usr/share/postgresql-common/pgdg/apt.postgresql.org.asc --fail (https://www.postgresql.org/media/keys/ACCC4CF8.asc)

## Create the repository configuration file

. /etc/os-release
sudo sh -c "echo 'deb [signed-by=/usr/share/postgresql-common/pgdg/apt.postgresql.org.asc] https://apt.postgresql.org/pub/repos/apt $VERSION_CODENAME-pgdg main' > /etc/apt/sources.list.d/pgdg.list"
sudo apt update
sudo apt -y install postgresql
