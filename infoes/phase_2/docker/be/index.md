FROM rust:1.75 AS builder

# Gerekli sistem bağımlılıklarını (örneğin, postgresql-client) ekle
RUN apt-get update && apt-get install -y postgresql-client

WORKDIR /backend

# Proje dosyalarını kopyala
COPY . /backend

# Projeyi derle (release modunda)
RUN cargo build --release

# Çalışma ortamı (örneğin, debian slim) imajından bir imaj oluştur
FROM debian:bullseye-slim

# Gerekli sistem bağımlılıklarını (örneğin, ca-certificates) ekle
RUN apt-get update && apt-get install -y ca-certificates postgresql-client

WORKDIR /backend

# Derleme aşamasından (builder) oluşturulan binary dosyayı kopyala
COPY --from=builder /backend/target/release/cms-backend /backend

# Uygulamanın çalışması için gerekli ortam değişkenlerini (örneğin, RUST_LOG) ayarlayabilirsin
ENV RUST_LOG=info

# Uygulamayı çalıştır
CMD ["trunk", "serve", "--release", "--address", "0.0.0.0"]