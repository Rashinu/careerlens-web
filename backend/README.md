# CareerLens

> AI destekli kariyer ve maaş analiz platformu — Türkiye pazarı odaklı.

CV yükle → AI analiz etsin → Maaşının piyasada nerede durduğunu öğren → 12 aylık kariyer yol haritanı al.

---

## Mimari

```
CareerLens/
├── CareerLens.API/           # ASP.NET Core 8 Web API + Razor Pages demo
├── CareerLens.Application/   # CQRS + MediatR (commands, queries, handlers)
├── CareerLens.Domain/        # Entity'ler, value objects, repository interfaces
├── CareerLens.Infrastructure/ # EF Core, Hangfire, Blob Storage, AI servisleri
├── CareerLens.Shared/        # DTO'lar, ApiResponse<T>, sabitler
└── CareerLens.Tests/         # xUnit unit testler (18 test, %100 geçiyor)
```

## Tech Stack

| Katman | Teknoloji |
|---|---|
| API | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 + PostgreSQL |
| Pattern | Clean Architecture + CQRS + MediatR |
| Auth | JWT (access + refresh token), BCrypt |
| AI | Semantic Kernel + OpenAI GPT-3.5/4o |
| Background | Hangfire + PostgreSQL storage |
| Storage | Azure Blob Storage (lokal geliştirmede dosya sistemi) |
| Cache | Redis (maaş benchmark sonuçları) |
| Logging | Serilog |
| Validation | FluentValidation (MediatR pipeline behaviour) |
| Test | xUnit + Moq + FluentAssertions |

---

## Kurulum (Lokal Geliştirme)

### Ön Koşullar
- .NET 8 SDK
- Docker Desktop

### 1. Repoyu klonla

```bash
git clone https://github.com/muratberatkeskin/careerlens.git
cd careerlens
```

### 2. Servisleri başlat (PostgreSQL + Redis)

```bash
docker compose up -d
```

> PostgreSQL: `localhost:5434` | Redis: `localhost:6380`

### 3. API'yi çalıştır

```bash
cd CareerLens.API
dotnet run --launch-profile http
```

API `http://localhost:5100` adresinde açılır.

- **Swagger:** `http://localhost:5100/swagger`
- **Demo UI:** `http://localhost:5100/Demo`
- **Health:** `http://localhost:5100/health`
- **Hangfire:** `http://localhost:5100/hangfire`

### 4. Testleri çalıştır

```bash
dotnet test CareerLens.Tests/CareerLens.Tests.csproj
```

---

## API Endpoint'leri

```
POST   /api/auth/register        # Yeni kullanıcı kaydı
POST   /api/auth/login           # Giriş, JWT token al
POST   /api/auth/refresh         # Token yenile

GET    /api/users/me             # Profil bilgisi [Authorize]
PUT    /api/users/me             # Profil güncelle [Authorize]

POST   /api/cv/upload            # CV yükle (PDF/DOCX, max 5MB) [Authorize]
GET    /api/cv/list              # CV listesi [Authorize]
GET    /api/cv/{id}/analysis     # AI analiz sonucu [Authorize]
POST   /api/cv/{id}/roadmap      # Kariyer roadmap oluştur [Authorize]
GET    /api/cv/{id}/roadmap      # Roadmap getir [Authorize]

GET    /api/salary/benchmark     # Maaş karşılaştırma (?position=&city=&years=)
POST   /api/salary/submit        # Anonim maaş girişi

GET    /api/dashboard            # Özet dashboard [Authorize]
GET    /health                   # Sağlık kontrolü
```

---

## CV Pipeline

```
Kullanıcı CV yükler
    ↓
Azure Blob Storage'a kaydedilir (dev: lokal dosya sistemi)
    ↓
Hangfire: CvTextExtractionJob
    PDF/DOCX → ham metin (PdfPig / DocumentFormat.OpenXml)
    ↓
Hangfire: CvAiParsingJob
    Ham metin → Semantic Kernel → GPT → Structured JSON
    ↓
CvAnalysis.Status: Analyzed
    ↓
Kullanıcı roadmap isteği gönderir
    ↓
Maaş benchmark + parsed CV → GPT → Gap analizi + öneriler
```

---

## Güvenlik

- CV parsing promptlarında injection koruması
- Maaş kayıtları anonim — UserId ile ilişkilendirilmez (KVKK)
- Azure Blob URL'leri SAS token ile (geçici erişim)
- Rate limiting tüm endpoint'lerde, AI endpoint'lerinde daha sıkı
- PII (CV içeriği) loglara yazılmaz
- Connection string ve API key'ler environment variable / Azure Key Vault

---

## Production Ortamı

```bash
# Azure Blob Storage
Azure__BlobStorage__ConnectionString=<connection-string>

# OpenAI
OpenAI__ApiKey=<api-key>
OpenAI__Model=gpt-4o

# PostgreSQL
ConnectionStrings__DefaultConnection=<pg-connection>

# Redis
ConnectionStrings__Redis=<redis-connection>

# JWT
Jwt__Secret=<min-32-karakter-gizli-anahtar>
```
