# CareerLens — AI Destekli Kariyer & Maaş Analiz Platformu
> CV yükle, nerede olduğunu öğren, nereye gideceğini gör.

---

## 📌 Vizyon

Türkiye'deki çalışanların büyük çoğunluğu maaşının piyasada nerede durduğunu bilmiyor. Salary Insights bu soruyu kısmen çözüyor — ama sadece anlık bir sayı veriyor. **CareerLens** bir adım ötesine geçiyor: CV'ni yükle, AI hem maaşını analiz etsin hem de kariyer yol haritanı çıkarsın.

---

## 🎯 Hedef Kitle

| Segment | Problem |
|---|---|
| Junior / Mid geliştiriciler | Maaşım az mı? Ne öğrensem zam alırım? |
| Kariyer değiştirmek isteyenler | Hangi alana geçsem, ne kadar sürer? |
| İş arayanlar | CV'im yeterli mi, nerede eksik? |
| HR / İşverenler (ileride) | Rakip şirketlere göre teklif nasıl olmalı? |

---

## 🔑 Core Özellikler (MVP)

### 1. CV Yükleme & Parsing
- PDF / DOCX yükleme
- AI ile otomatik parsing: pozisyon, deneyim yılı, teknoloji stack, eğitim
- Manuel düzenleme imkanı

### 2. Maaş Analizi
- Pozisyon + sektör + deneyim + şehir bazlı maaş aralığı
- P25 / P50 / P75 dağılımı
- "Şu an piyasada neredesin?" skoru
- Anonim veri havuzuyla karşılaştırma

### 3. AI Kariyer Koçu ⭐ (Rakip farkı)
- CV'deki eksiklikleri tespit et
- "Bu profille hangi pozisyonlara başvurabilirsin?"
- "2 yıl içinde %30 zam için ne öğrenmelisin?"
- Öğrenme yol haritası önerisi (kurs, sertifika, teknoloji)
- Hedef pozisyona göre gap analizi

### 4. Anonim Veri Katkısı
- Kullanıcılar kendi maaşlarını anonim olarak girebilir
- Veri havuzu büyüdükçe analiz kalitesi artar
- Gamification: veri girenler premium özellik açar

---

## 🏗️ Teknik Mimari

### Backend — .NET 8 Web API
```
CareerLens/
├── CareerLens.API/              # ASP.NET Core Web API
├── CareerLens.Application/      # CQRS + MediatR (Business Logic)
├── CareerLens.Domain/           # Entity'ler, Value Objects
├── CareerLens.Infrastructure/   # DB, AI, Storage implementasyonları
└── CareerLens.Shared/           # DTO'lar, sabitler
```

**Kullanılacak teknolojiler:**
- ASP.NET Core 8 Web API
- Entity Framework Core + PostgreSQL
- MediatR (CQRS pattern)
- JWT Authentication
- Hangfire (arka plan işlemleri)
- Serilog (loglama)

### AI Katmanı
- **Semantic Kernel** (.NET native AI orchestration)
- **OpenAI GPT-4o** — CV parsing, kariyer analizi, öneri üretme
- **Prompt Engineering** — Her analiz tipi için ayrı prompt şablonları
- CV parsing için structured output (JSON response)

### Frontend
- **Başlangıç:** Razor Pages veya minimal Blazor (backend odaklı kalabilmek için)
- **İleride:** React / Next.js (ayrı bir frontend developer ile)

### Storage & Infra
- **Azure Blob Storage** — CV dosyaları
- **PostgreSQL** — Ana veritabanı
- **Redis** — Cache (maaş sorgu sonuçları)
- **Azure App Service** — Hosting

---

## 📊 Veri Modeli (Temel)

```
User
├── Id, Email, PasswordHash
├── Plan (Free / Pro)
└── CreatedAt

CvAnalysis
├── Id, UserId
├── RawFileUrl (Azure Blob)
├── ParsedData (JSON) ← AI çıktısı
├── AnalysisResult (JSON)
└── CreatedAt

SalaryRecord (anonim havuz)
├── Id
├── Position, Sector, City
├── YearsOfExperience
├── NetSalary
├── TechStack (array)
└── SubmittedAt

CareerRoadmap
├── Id, UserId, CvAnalysisId
├── CurrentScore
├── TargetPosition
├── GapAnalysis (JSON)
├── Recommendations (JSON)
└── GeneratedAt
```

---

## 💰 Monetizasyon (SaaS Modeli)

| Plan | Fiyat | Özellikler |
|---|---|---|
| **Free** | Ücretsiz | Aylık 1 CV analizi, temel maaş karşılaştırma |
| **Pro** | ₺199/ay | Sınırsız analiz, AI kariyer koçu, detaylı roadmap |
| **HR** | ₺999/ay | Toplu analiz, benchmark raporu, API erişimi |

**Ek gelir kaynakları:**
- Kurs platformları ile affiliate (roadmap önerilerinde)
- İş ilanı yönlendirme (ileride)
- Şirketlere benchmark raporu satışı

---

## 🗓️ Geliştirme Yol Haritası

### Faz 1 — MVP (0-3 ay)
- [ ] Proje kurulumu, Clean Architecture
- [ ] User authentication (JWT)
- [ ] CV upload (Azure Blob)
- [ ] AI ile CV parsing (Semantic Kernel + OpenAI)
- [ ] Temel maaş karşılaştırma (seed data ile başla)
- [ ] Basit sonuç sayfası

### Faz 2 — AI Kariyer Koçu (3-5 ay)
- [ ] Gap analizi promptları
- [ ] Kariyer yol haritası üretimi
- [ ] Anonim veri katkı sistemi
- [ ] Dashboard UI geliştirme

### Faz 3 — SaaS (5-8 ay)
- [ ] Stripe entegrasyonu (ödeme)
- [ ] Plan yönetimi, rate limiting
- [ ] HR modülü
- [ ] SEO optimizasyonu
- [ ] Beta kullanıcı lansmanı

### Faz 4 — Büyüme (8-12 ay)
- [ ] API erişimi (B2B)
- [ ] Şirket profilleri
- [ ] İş ilanı entegrasyonu
- [ ] Mobile PWA

---

## 🔐 Güvenlik & Gizlilik

- CV dosyaları şifreli Azure Blob'da, kullanıcıya özel erişim
- Maaş verileri tamamen anonim — kullanıcı kimliğiyle ilişkilendirilmez
- KVKK uyumu (Türkiye için zorunlu)
- Rate limiting tüm endpoint'lerde
- Input validation + sanitization (CV içeriği için)

---

## 📈 Başarı Metrikleri

| Metrik | Hedef (6. ay) |
|---|---|
| Kayıtlı kullanıcı | 500+ |
| Aylık CV analizi | 200+ |
| Anonim maaş kaydı | 1.000+ |
| Pro dönüşüm oranı | %5+ |
| Aylık gelir | ₺5.000+ |

---

## 🚀 Rakip Analizi

| Platform | Güçlü Yön | Zayıf Yön |
|---|---|---|
| Salary Insights TR | Geniş Türkiye verisi | AI yok, kariyer koçu yok |
| LinkedIn Salary | Global veri | Türkiye verisi zayıf, CV analizi yok |
| Glassdoor | Şirket yorumları | Türkiye'de yetersiz |
| **CareerLens** | AI + CV + Roadmap | Yeni, veri birikimi zaman alır |

---

## 💡 Neden Bu Proje Portföy İçin Güçlü?

1. **Gerçek problem çözüyor** — binlerce kişinin ihtiyacı var
2. **Full stack showcase** — .NET API + AI + Auth + Payment + Cloud
3. **Üretimde çalışıyor** — gerçek kullanıcı, gerçek veri
4. **Ölçeklenebilir mimari** — Clean Architecture, CQRS
5. **AI entegrasyonu** — 2025-2026'nın en çok aranan becerisi
6. **SaaS deneyimi** — ürün geliştirme sürecinin tamamını öğrenirsin

---

*Son güncelleme: Mayıs 2026*
