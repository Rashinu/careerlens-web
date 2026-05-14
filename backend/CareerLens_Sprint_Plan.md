# CareerLens — Faz 1 MVP Sprint Planı
> Süre: 12 Hafta | Günlük çalışma: ~2.5-3 saat (18:30-23:00 arası)

---

## 🗓️ Genel Bakış

| Faz | Hafta | Konu |
|---|---|---|
| Temel | 1-2 | Proje kurulumu + Clean Architecture |
| Auth | 3-4 | Kullanıcı sistemi + JWT |
| CV | 5-6 | Dosya yükleme + Azure Blob |
| AI | 7-9 | CV parsing + Semantic Kernel |
| Maaş | 10-11 | Maaş karşılaştırma + seed data |
| Bitiş | 12 | Test + deploy + GitHub polish |

---

## 📅 Haftalık Sprint Detayları

---

### 🔧 HAFTA 1 — Proje Kurulumu & Mimari

**Hedef:** Çalışan bir iskelet, hiçbir özellik yok ama her katman yerli yerinde.

**Görevler:**
- [ ] GitHub repo aç, `.gitignore` + `README.md` yaz
- [ ] Solution yapısını kur:
  - `CareerLens.API`
  - `CareerLens.Application`
  - `CareerLens.Domain`
  - `CareerLens.Infrastructure`
  - `CareerLens.Shared`
- [ ] NuGet paketlerini yükle: MediatR, EF Core, Serilog, FluentValidation
- [ ] `appsettings.json` yapısını kur (connection string, JWT config, Azure config)
- [ ] Health check endpoint ekle (`GET /health`)
- [ ] PostgreSQL bağlantısını test et (local Docker ile)

**Hafta sonu kontrolü:** `dotnet run` çalışıyor, `/health` 200 dönüyor ✅

---

### 🔧 HAFTA 2 — Domain & Veritabanı

**Hedef:** Tüm entity'ler ve migration hazır.

**Görevler:**
- [ ] Domain entity'lerini yaz:
  - `User`
  - `CvAnalysis`
  - `SalaryRecord`
  - `CareerRoadmap`
- [ ] EF Core DbContext kur
- [ ] İlk migration oluştur ve uygula
- [ ] Repository pattern kur (generic + specific)
- [ ] Unit of Work ekle
- [ ] Seed data yaz (test için 50-100 anonim maaş kaydı)

**Hafta sonu kontrolü:** Migration çalışıyor, tablolar oluştu ✅

---

### 🔐 HAFTA 3 — Authentication (Register & Login)

**Hedef:** Kullanıcı kayıt ve giriş sistemi çalışıyor.

**Görevler:**
- [ ] `RegisterCommand` + handler (MediatR)
- [ ] `LoginCommand` + handler
- [ ] JWT token üretimi (access token)
- [ ] Password hashing (BCrypt)
- [ ] `POST /api/auth/register` endpoint
- [ ] `POST /api/auth/login` endpoint
- [ ] FluentValidation kuralları (email format, şifre güçlülüğü)
- [ ] Swagger'da JWT desteği

**Hafta sonu kontrolü:** Postman ile register + login çalışıyor, token geliyor ✅

---

### 🔐 HAFTA 4 — Auth Tamamlama + Kullanıcı Profili

**Hedef:** Refresh token + profil endpoint'leri hazır.

**Görevler:**
- [ ] Refresh token sistemi
- [ ] `[Authorize]` middleware kurulumu
- [ ] `GET /api/users/me` — profil bilgisi
- [ ] `PUT /api/users/me` — profil güncelleme
- [ ] Global exception handler (middleware)
- [ ] API response wrapper (`ApiResponse<T>`)
- [ ] Rate limiting (AspNetCoreRateLimit)

**Hafta sonu kontrolü:** Token yenileme çalışıyor, yetkisiz istek 401 dönüyor ✅

---

### 📁 HAFTA 5 — CV Yükleme & Azure Blob Storage

**Hedef:** Kullanıcı CV yükleyebiliyor, dosya Azure'da saklanıyor.

**Görevler:**
- [ ] Azure Storage Account kur (free tier yeterli)
- [ ] `Azure.Storage.Blobs` NuGet paketi
- [ ] `IBlobStorageService` interface + implementasyon
- [ ] Dosya validasyonu (sadece PDF/DOCX, max 5MB)
- [ ] `POST /api/cv/upload` endpoint
- [ ] Upload sonrası DB'ye `CvAnalysis` kaydı oluştur (status: `Uploaded`)
- [ ] `GET /api/cv/list` — kullanıcının CV listesi
- [ ] Güvenli URL üretimi (SAS token ile geçici erişim)

**Hafta sonu kontrolü:** Postman ile CV yükle, Azure portalda dosyayı gör ✅

---

### 📁 HAFTA 6 — CV Dosya İşleme & Text Extraction

**Hedef:** Yüklenen CV'den ham metin çıkarılıyor.

**Görevler:**
- [ ] PDF metin çıkarma: `PdfPig` NuGet paketi
- [ ] DOCX metin çıkarma: `DocumentFormat.OpenXml`
- [ ] `ICvParserService` interface
- [ ] Çıkarılan metni DB'ye kaydet (`ParsedRawText`)
- [ ] Hangfire kurulumu (arka plan işleri için)
- [ ] CV yükleme → text extraction background job olarak çalışsın
- [ ] `CvAnalysis` status güncelleme: `Uploaded` → `TextExtracted`

**Hafta sonu kontrolü:** CV yükle, dakika içinde ham metin DB'de görünsün ✅

---

### 🤖 HAFTA 7 — Semantic Kernel Kurulumu & İlk Prompt

**Hedef:** Semantic Kernel çalışıyor, OpenAI'a ilk istek gidiyor.

**Görevler:**
- [ ] `Microsoft.SemanticKernel` NuGet paketi
- [ ] OpenAI API key konfigürasyonu (Azure Key Vault veya appsettings)
- [ ] `ICareerAiService` interface tasarımı
- [ ] İlk prompt şablonu — CV parsing için:
  ```
  Aşağıdaki CV metnini analiz et ve JSON formatında döndür:
  - fullName, email, phone
  - currentPosition, yearsOfExperience
  - techStack (array)
  - education (array)
  - languages (array)
  ```
- [ ] Structured output parsing (JSON deserialize)
- [ ] Test: 3 farklı CV formatıyla dene

**Hafta sonu kontrolü:** Ham CV metni → structured JSON çıkıyor ✅

---

### 🤖 HAFTA 8 — AI CV Analizi Tamamlama

**Hedef:** AI analiz sonuçları DB'ye kaydediliyor, endpoint hazır.

**Görevler:**
- [ ] CV parsing background job'a entegre et
- [ ] `ParsedData` (JSON) DB'ye kaydet
- [ ] `CvAnalysis` status: `TextExtracted` → `Analyzed`
- [ ] `GET /api/cv/{id}/analysis` — analiz sonucunu getir
- [ ] Prompt güvenliği: injection koruması
- [ ] Token kullanım loglama (maliyet takibi için)
- [ ] Hata yönetimi: OpenAI timeout / rate limit durumları
- [ ] Retry policy (Polly kütüphanesi)

**Hafta sonu kontrolü:** CV yükle → 1-2 dakika içinde analiz sonucu endpoint'ten geliyor ✅

---

### 🤖 HAFTA 9 — Kariyer Koçu Promptları

**Hedef:** AI kariyer önerisi ve gap analizi üretiyor.

**Görevler:**
- [ ] Gap analizi prompt şablonu:
  ```
  Kullanıcı profili: {parsedCvData}
  Hedef pozisyon: {targetPosition}
  Piyasa verisi: {salaryBenchmark}
  
  Analiz et:
  1. Mevcut profil bu pozisyon için ne kadar uygun? (0-100 skor)
  2. Eksik beceriler neler?
  3. 12 aylık öğrenme yol haritası
  4. Tahmini maaş aralığı
  ```
- [ ] `POST /api/cv/{id}/roadmap` — hedef pozisyon gönder, roadmap al
- [ ] Roadmap DB'ye kaydet
- [ ] `GET /api/cv/{id}/roadmap` — kaydedilmiş roadmap getir
- [ ] Öneri kalitesi testi: 5 farklı profille dene, sonuçları değerlendir

**Hafta sonu kontrolü:** "Senior .NET Developer olmak istiyorum" → detaylı roadmap geliyor ✅

---

### 💰 HAFTA 10 — Maaş Karşılaştırma Modülü

**Hedef:** Kullanıcı maaşını piyasayla karşılaştırabiliyor.

**Görevler:**
- [ ] Seed data'yı genişlet (200+ kayıt, farklı pozisyon/şehir/sektör)
- [ ] `SalaryBenchmarkQuery` — pozisyon + şehir + deneyim ile sorgula
- [ ] P25 / P50 / P75 hesaplama
- [ ] `GET /api/salary/benchmark?position=...&city=...&years=...`
- [ ] "Sen piyasanın kaçıncı yüzdeliğindesin?" skoru
- [ ] CV analizi ile otomatik bağlantı: parsing sonrası otomatik benchmark çalıştır
- [ ] `POST /api/salary/submit` — anonim maaş girişi

**Hafta sonu kontrolü:** ".NET Developer, İstanbul, 3 yıl" → P25/P50/P75 geliyor ✅

---

### 💰 HAFTA 11 — Entegrasyon & Kullanıcı Akışı

**Hedef:** Uçtan uca akış çalışıyor: kayıt → CV yükle → analiz → roadmap → maaş karşılaştırma.

**Görevler:**
- [ ] Tüm endpoint'leri baştan sona test et (Postman collection)
- [ ] Eksik validasyonları tamamla
- [ ] Background job monitoring (Hangfire dashboard)
- [ ] Loglama gözden geçir (hassas veri loglanıyor mu?)
- [ ] DB index optimizasyonu (sık sorgulanan alanlar)
- [ ] Basit bir HTML demo sayfası (Razor Pages — sadece akışı göstermek için)
- [ ] `GET /api/dashboard` — kullanıcının tüm analizlerinin özeti

**Hafta sonu kontrolü:** Sıfırdan kayıt ol, CV yükle, roadmap al — hepsi çalışıyor ✅

---

### 🚀 HAFTA 12 — Deploy & GitHub Polish

**Hedef:** Proje canlıda, GitHub portföy hazır.

**Görevler:**
- [ ] Azure App Service'e deploy (free tier)
- [ ] Environment variable'ları Azure'da konfigüre et
- [ ] GitHub Actions CI pipeline (push → otomatik deploy)
- [ ] README.md'yi güncelle:
  - Proje açıklaması
  - Mimari diyagram
  - Kurulum adımları
  - Demo linki
  - Ekran görüntüleri
- [ ] Swagger UI canlıda açık olsun
- [ ] `.env.example` dosyası (API key'ler olmadan)
- [ ] Kodun son gözden geçirmesi (dead code temizliği)

**Hafta sonu kontrolü:** GitHub linki paylaşılabilir, demo URL çalışıyor ✅

---

## 📊 Haftalık Çalışma Dağılımı

Programına göre (.NET/GoIT zamanların):

| Gün | Saat | Süre | Ne yapacaksın |
|---|---|---|---|
| Pazartesi | 18:30-20:00 | 1.5 saat | O haftanın görevi |
| Pazartesi | 20:00-21:30 | 1.5 saat | Devam |
| Salı | 20:00-21:30 | 1.5 saat | Devam / hata çözme |
| Çarşamba | 20:00-23:00 | 3 saat | Yoğun geliştirme |
| Perşembe | 18:30-20:00 | 1.5 saat | Devam |
| Perşembe | 20:00-21:30 | 1.5 saat | Devam |
| Cumartesi | 18:00-20:00 | 2 saat | Hafta özeti + test |

**Haftalık toplam: ~12 saat** → 12 hafta = ~144 saat geliştirme

---

## ⚠️ Riskler & Çözümler

| Risk | İhtimal | Çözüm |
|---|---|---|
| OpenAI maliyeti | Orta | GPT-3.5 ile başla, GPT-4o sonra |
| DGS sınavı dönemi | Yüksek | 10-11. haftalarda tempo düşür |
| Bir hafta geride kalma | Yüksek | Her sprint esnek, 1 hafta buffer var |
| Azure kurulumu zorluğu | Düşük | Azure free tier + dokümantasyon yeterli |

---

## 🏁 Faz 1 Bitince Elinde Ne Olacak?

- ✅ Canlıda çalışan bir uygulama (gerçek URL)
- ✅ Clean Architecture ile yazılmış .NET 8 API
- ✅ AI entegrasyonu (Semantic Kernel + OpenAI)
- ✅ Azure cloud deployment deneyimi
- ✅ JWT Auth, background jobs, blob storage
- ✅ GitHub'da gösterilebilir portföy projesi
- ✅ Faz 2'ye (SaaS + ödeme) hazır temel

---

*Güncelleme: Mayıs 2026*
