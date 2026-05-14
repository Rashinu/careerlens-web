# CareerLens — Claude Code Bağlamı

AI destekli kariyer ve maaş analiz platformu. Türkiye pazarı odaklı.

## Mimari

```
CareerLens/
├── CareerLens.API/           # ASP.NET Core 8 Web API (controllers, middleware, filters)
├── CareerLens.Application/   # CQRS + MediatR (commands, queries, handlers, validators)
├── CareerLens.Domain/        # Entity'ler, value objects, domain events
├── CareerLens.Infrastructure/ # DB, AI, Storage implementasyonları (EF Core, Hangfire, Blob)
└── CareerLens.Shared/        # DTO'lar, response wrapper, sabitler
```

## Tech Stack

| Katman | Teknoloji |
|---|---|
| API | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 + PostgreSQL |
| Pattern | CQRS + MediatR |
| Auth | JWT (access + refresh token), BCrypt |
| AI | Semantic Kernel + OpenAI GPT-4o |
| Background | Hangfire |
| Storage | Azure Blob Storage (SAS token ile güvenli URL) |
| Cache | Redis |
| Logging | Serilog |
| Validation | FluentValidation |
| Resilience | Polly (retry, circuit breaker) |
| Frontend (MVP) | Razor Pages (minimal, demo amaçlı) |

## Domain Modeli

```
User          → Id, Email, PasswordHash, Plan (Free/Pro), RefreshToken
CvAnalysis    → Id, UserId, RawFileUrl, ParsedRawText, ParsedData(JSON), Status, CreatedAt
SalaryRecord  → Id, Position, Sector, City, YearsOfExperience, NetSalary, TechStack[], SubmittedAt
CareerRoadmap → Id, UserId, CvAnalysisId, CurrentScore, TargetPosition, GapAnalysis(JSON), Recommendations(JSON)
```

### CvAnalysis Status Akışı
`Uploaded` → `TextExtracted` → `Analyzed`

## CQRS Kuralları

- Her özellik: `Command` (yazar) veya `Query` (okur) + `Handler` + `Validator`
- Handler'lar sadece `IRepository` ve `IService` interface'lerine bağımlı — doğrudan DbContext kullanmaz
- Validation her zaman FluentValidation ile, handler içinde değil

## API Endpoint'leri (MVP)

```
POST   /api/auth/register
POST   /api/auth/login
POST   /api/auth/refresh
GET    /api/users/me
PUT    /api/users/me
POST   /api/cv/upload           (PDF/DOCX, max 5MB, [Authorize])
GET    /api/cv/list             ([Authorize])
GET    /api/cv/{id}/analysis    ([Authorize])
POST   /api/cv/{id}/roadmap     ([Authorize], hedef pozisyon gönder)
GET    /api/cv/{id}/roadmap     ([Authorize])
GET    /api/salary/benchmark    (?position=&city=&years=)
POST   /api/salary/submit       (anonim)
GET    /api/dashboard           ([Authorize])
GET    /health
```

## Güvenlik Kuralları (builder + reviewer dikkat)

- CV parsing promptlarında injection koruması zorunlu
- Maaş kayıtları UserId ile asla ilişkilendirilmez (anonim)
- Azure Blob URL'leri her zaman SAS token ile, direkt erişim yok
- Rate limiting tüm endpoint'lerde (public olanlar daha katı)
- AI endpoint'leri için ayrı, daha sıkı rate limiting
- Token kullanımı her AI çağrısında loglanır (maliyet takibi)
- PII (CV içeriği) loglara yazılmaz

## AI / Semantic Kernel

- `ICareerAiService` interface — tüm AI operasyonları bu üzerinden
- Her analiz tipi için ayrı prompt şablonu (CV parsing, gap analizi, roadmap)
- OpenAI structured output (JSON response format)
- Polly ile retry (timeout + rate limit)
- GPT-3.5-turbo ile başla, GPT-4o ilerleyen fazda

## Response Formatı

Tüm endpoint'ler `ApiResponse<T>` wrapper döner:
```json
{ "success": true, "data": {}, "error": null }
{ "success": false, "data": null, "error": "mesaj" }
```

## Test Yaklaşımı

- **Unit:** Handler'lar, service'ler (mock repository ile)
- **Integration:** Controller → handler → DB (real PostgreSQL, test container)
- **Kapsam:** Genel %80+, auth/security %100
- Test projesi: `CareerLens.Tests`

## Önemli Kısıtlamalar

- Hiçbir zaman connection string veya API key koda yazılmaz (environment variable / Azure Key Vault)
- EF Core migration'ları her zaman `Infrastructure` projesinde
- Background job'lar (Hangfire) her zaman idempotent yazılır
- CV dosyaları parse edilmeden önce MIME type + boyut validasyonu yapılır
