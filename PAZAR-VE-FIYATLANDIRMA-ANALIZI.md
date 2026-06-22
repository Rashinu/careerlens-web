# CareerLens — Pazar Analizi ve Fiyatlandırma Stratejisi

**Tarih:** 21 Haziran 2026 (güncelleme: 22 Haziran 2026 — SalaryInsights canlı site incelemesi sonrası)

---

## 0. Güncelleme Notu (22 Haziran)

İlk analiz, SalaryInsights'ı yalnızca metin tabanlı bir özetten (WebFetch) değerlendirmişti ve onu küçümsemişti. Siteyi Playwright ile bizzat gezdikten sonra ortaya çıkan gerçek tablo çok daha rekabetçi:

- **50.800+ anonim maaş verisi**, ayda 30.000+ ziyaretçi — bizim 200 sentetik kayıtla kıyaslanamaz bir ölçek.
- **Kendi CV analizi + AI eşleştirme özelliği var** ("CV'ye Göre Maaş Önerisi") — CV yükle, AI pozisyon/sektör/deneyimi algılar, P25/P50/P75 gösterir. **Bizim "tek farkımız CV analizi" iddiamız artık doğru değil** — onlar da yapıyor. Bizim gerçek farkımız: onların CV analizi sadece maaş tahmini ile bitiyor, **gap analizi + adım adım roadmap + maaş etkili öneriler yok**.
- **Kredi bazlı CV analizi** ("1 kredi · Hesap gerektirir") — abonelik değil, kullanım bazlı tek seferlik ödeme modeli kullanıyorlar.
- **Ücretli PDF raporlar**: 149-199 TL tek seferlik (örn. "Top 25 Pozisyon Maaş Raporu"), **9.900 TL+KDV** çeyreklik kapsamlı kurumsal rapor (330 pozisyon, 61 şehir, 124 sektör).
- **Kurumsal paket** ("En Popüler" rozetiyle öne çıkarılmış): "Aylık 30 pozisyona kadar maaş ve yan hak sorgulama" — İK ekiplerine Excel yükle, AI eşleştirsin, piyasa altı maaşları tespit et.
- **Belge ile maaş doğrulama**: Bordro/SGK dökümü yükleyerek veri güvenilirliğini artırma seçeneği — bizde olmayan bir güven mekanizması.
- **Devasa SEO yatırımı**: Her pozisyon için ayrı landing page (`/maas/yazilim-muhendisi` gibi 30+ sayfa), çok detaylı FAQ bölümü (15+ soru, spesifik maaş rakamlarıyla).
- **"Nasıl Çalışır" tasarımı sade**: Bağlayıcı çizgi YOK, sadece 3 izole numaralı daire. Ayrıca ayrı bir "Metodolojimiz" bloğu var (Net Maaş Normalizasyonu, İstatistiksel Doğruluk, Gizlilik & Güvenlik, Piyasa İçgörüleri) — bizim kurduğumuz güven şeridiyle aynı mantık, doğru yöndeydik.

Aşağıdaki bölümler bu güncel bilgiyle yeniden yazıldı.

---

## 1. Rakip Haritası

| Rakip | Çekirdek Ürün | Ücretsiz Sunduğu | Parayla Sattığı | Bilinen Fiyat |
|---|---|---|---|---|
| **SalaryInsights.com.tr** | Anonim maaş benchmark + AI CV-maaş eşleştirme (TR) | Temel P25/P50/P75 görüntüleme (referans kodu ile), anonim paylaşım | **Kredi bazlı CV analizi**, tek seferlik PDF raporlar, çeyreklik kurumsal rapor, kurumsal Excel/API paketi | CV analizi: kredi sistemi · Raporlar: 149-199 TL · Kurumsal rapor: 9.900 TL+KDV |
| **LinkedIn Premium Career** | Profil görünürlüğü + iş başvuru zekası | Temel profil, iş ilanı arama | "Kim profilime baktı", maaş içgörüleri, başvuru istatistikleri, AI mülakat hazırlığı | ~110-150 TL/ay (TR) |
| **kariyer.net** | İş ilanı + CV barındırma | CV oluşturma, başvuru, temel ilan görme | **İş arayan tarafı esasen ücretsiz** — para işveren ilan ücretlerinden geliyor | İşveren paketleri 1.299 TL+ / ay |
| **levels.fyi / Glassdoor (global referans)** | Şirket bazlı maaş + seviye verisi | Temel maaş verisi görüntüleme | Detaylı veri seti (Google Sheet export), 1:1 maaş pazarlığı koçluğu | Koçluk paketleri $100-600+ |

### Çıkarılan Desen (Düzeltilmiş)

Temel maaş verisi görüntüleme hâlâ herkeste ücretsiz — bu değişmedi. Ama **AI/CV destekli derin analiz** artık ücretsiz değil: SalaryInsights bunu kredi bazlı satıyor, LinkedIn aboneliğe bağlıyor. Para genel olarak üç yerden geliyor: (a) **AI/CV analizi gibi maliyetli işlemler** (kredi veya abonelik), (b) **B2B/kurumsal** (Excel toplu analiz, API, çeyreklik rapor), (c) **tek seferlik içerik satışı** (PDF rapor). CareerLens'in konumlanması için kritik soru artık "ücretsiz mi paralı mı" değil, **"hangi model: kredi mi, abonelik mi"** — bkz. Bölüm 4.

---

## 2. CareerLens'in Maliyet Yapısı (Fiyatlandırmanın Temeli)

Fiyatlandırma keyfi olmamalı — gerçek değişken maliyete dayanmalı:

| Özellik | Değişken Maliyet | Sabit/Sıfır Maliyet |
|---|---|---|
| Maaş benchmark görüntüleme | Yok (Postgres sorgusu) | ✅ |
| Maaş paylaşımı | Yok | ✅ |
| TÜFE enflasyon düzeltmesi | Yok (cache'li TCMB çağrısı, 7 günlük) | ✅ |
| **CV analizi (AI parse)** | **OpenAI API çağrısı** (~1000 token) | ❌ |
| **Roadmap üretimi (AI)** | **OpenAI API çağrısı** (~2000 token) | ❌ |

Sonuç: **Tek gerçek maliyet kalemi AI çağrıları.** Fiyatlandırma stratejisi bu ikisinin etrafında dönmeli — gerisi (maaş verisi) zaten bedava sunulabilir çünkü bedavaya maliyeti yok ve veri ağını büyütüyor.

---

## 3. Ne Ücretsiz Kalmalı, Ne Paralı Olmalı

### Ücretsiz Tutulmalı (rakiplerle de uyumlu, veri ağı için kritik)
- Maaş benchmark görüntüleme (P25/P50/P75)
- Maaş verisi paylaşımı (zaten anonim, bunu paralı yapmak veri akışını öldürür)
- Hesap oluşturma, dashboard
- **1 adet** CV analizi + roadmap (deneme/hook — kullanıcı ürünün değerini görmeden ödeme yapmaz)

### Paralı Olmalı (gerçek maliyeti olan, kullanıcının tekrar tekrar istediği)
- **Sınırsız/ek CV analizi** (farklı CV versiyonları, farklı pozisyonlar için)
- **Roadmap yeniden üretimi** (farklı hedef pozisyonlar denemek — "Backend'den Senior'a" vs "Backend'den Tech Lead'e")
- PDF/paylaşılabilir rapor export (LinkedIn'e koyacak, işverene gösterecek format)
- Geçmiş roadmap karşılaştırması ("3 ay önceki skorum 65 idi, şimdi 78")
- Öncelikli AI işleme (kuyrukta beklemeden, GPT-4o gibi daha güçlü model)

---

## 4. Önerilen Fiyatlandırma Modeli

### Kredi mi, Abonelik mi? (SalaryInsights kredi kullanıyor)

SalaryInsights'ın CV analizini **kredi bazlı** (abonelik değil) satması tesadüf değil — kullanıcı davranışına uygun: çoğu kişi CV analizini ayda 1-2 kez yapar (yeni iş ararken), sürekli değil. Abonelik bu kullanım deseniyle uyumsuz ve "iptal etmeyi unutursam" kaygısı yaratır; kredi ise "ne kadar kullanırsam o kadar öderim" hissi verir, satın alma engelini düşürür.

**CareerLens için öneri: Hibrit model**, ikisinin de güçlü yanını alarak:
- **Kredi paketi** (tek seferlik): 3 analiz/roadmap kredisi = ₺99 — düşük taahhütle deneme/ara sıra kullanım için (SalaryInsights'a karşı doğrudan rekabet).
- **Pro abonelik** (tekrarlayan kullanıcı için): ₺149/ay sınırsız — aktif iş arayan, birden fazla pozisyon deneyen kullanıcı için (kredi almaktan daha ucuza gelir, 2 analizden sonra başa baş).

Bu ikisini bir arada sunmak, SalaryInsights'ın tek-kredi modeline karşı esneklik avantajı sağlar; "düşük taahhüt isteyen" ile "yoğun kullanan" kullanıcıyı aynı anda yakalar.

| Plan | Fiyat | İçerik |
|---|---|---|
| **Free** | ₺0 | Sınırsız maaş benchmark + paylaşım, ayda 1 CV analizi + 1 roadmap |
| **Kredi Paketi** | ₺99 / 3 kredi | Tek seferlik, taahhütsüz — ara sıra kullanım |
| **Pro** | **₺149/ay** veya **₺1.490/yıl** (~%17 indirim) | Sınırsız CV analizi + roadmap üretimi, PDF export, geçmiş roadmap karşılaştırması, GPT-4o öncelikli işleme |

**Neden ₺149/ay:** LinkedIn Premium Career'ın (~110-150 TL) hemen üstünde/yanında konumlanıyor — kullanıcı zaten bu fiyat aralığını "kariyer aracı" kategorisi için normal görüyor (referans fiyat etkisi). Çok daha yüksek (₺300+) erken aşamada güven inşa etmeden caydırıcı olur; çok daha düşük (₺49-79) ise "ucuz/güvenilmez" sinyali verip AI maliyetini karşılamayabilir.

**Maliyet kontrolü:** GPT-3.5-turbo ile bir analiz+roadmap çifti ~$0.01-0.03 maliyetli (mevcut prompt boyutlarına göre). Aylık 10 analiz yapan bir Pro kullanıcı size ~$0.30 maliyet çıkarır, ₺149 (~$4-5) gelir getirir — sağlıklı marj. ₺99/3 kredi paketinde kredi başına ~₺33 — marj çok daha yüksek (taahhütsüz kullanım için fiyat primi normal).

**B2B not:** SalaryInsights'ın kurumsal paketi (Excel toplu analiz, ~aylık ücret) ve çeyreklik raporu (9.900 TL+KDV) ciddi gelir kalemleri gibi görünüyor. CareerLens'in roadmap/gap-analizi B2B'ye (bootcamp, outplacement) taşınabilir potansiyeli daha önce not edilmişti (bkz. önceki oturum B2B2C tartışması) — bu rakip verisi o yönün gerçek bir gelir potansiyeli olduğunu doğruluyor.

### TL bazlı, enflasyona endeksli
Türkiye'de dolar bazlı fiyat kullanıcıyı ürkütür ve döviz kuru dalgalanmasında ani fiyat artışı gibi görünür. TL fiyat + **kendi TÜFE entegrasyonunuzu kullanarak yıllık otomatik fiyat güncellemesi** ("CareerLens Pro fiyatı enflasyona göre güncellenmiştir" şeffaflığı) — zaten elinizde olan bir özelliği pazarlama/güven aracına çevirir.

---

## 5. Farklılaşma Fırsatları (Rakiplerin Sunmadığı)

> **Düzeltme:** SalaryInsights'ın da AI destekli CV→maaş eşleştirmesi olduğu için "CV analizi yapan tek platform" iddiası artık geçersiz. Gerçek farkımız, CV analizinin **nereye kadar gittiği**.

1. **Gap analizi + adım adım roadmap** — SalaryInsights CV'den maaş tahmini çıkarıp duruyor; kariyer gelişim planı, güçlü/eksik yön analizi, "şimdi/3-6 ay/6-12 ay" zaman çizelgesi sunmuyor. Bu hâlâ bizim asıl, gerçek farkımız.
2. **Maaş etkisi tahminli öneriler** ("+₺5.000/ay") — her öneriye somut maaş etkisi bağlamak hiçbir rakipte yok. Pro plan'ın "hero" özelliği olarak pazarlanmalı.
3. **"Skorun nasıl değişti" zaman çizelgesi** — SalaryInsights'ın referans kodu statik bir sonuca dönüyor; CareerLens kullanıcının gelişimini zamanla izleyebilir (roadmap geçmişi). Retention/ücretli plan motoru olabilir.
4. **Tek platformda CV→analiz→maaş→roadmap döngüsü** — kariyer.net sadece ilan, LinkedIn sadece ağ; SalaryInsights maaş+CV analizini birleştirdi ama roadmap'i hiç yok. CareerLens'in entegre olması (maaş + CV + roadmap üçü bir arada) hâlâ bir farklılaşma; "parçalı değil bütünsel" mesajı pazarlamada kullanılmalı.

### Değerlendirilmesi Gereken (Rakipte var, bizde yok)
- **Belge ile maaş doğrulama** (bordro/SGK dökümü yükleme) — veri güvenilirliğini artıran bir özellik; orta vadede değerlendirilebilir, şimdilik öncelik değil.
- **Pozisyon bazlı SEO landing page'leri** — SalaryInsights'ın organik trafiğinin büyük kısmı muhtemelen buradan geliyor; homepage'den sonraki adım olarak düşünülebilir.

---

## 6. Farklılaşma Yol Haritası — Adım Adım

Sıralama keyfi değil: önce **doğrulanmış, ucuz, mevcut altyapıyla yapılabilen** farklar; sonra **maliyetli/riskli bahisler**. "Sesli AI" gibi pahalı bir özelliği, henüz ödeyen kullanıcısı olmayan bir ürüne en başta eklemek — para kazanma motorunu kanıtlamadan maliyet eklemek anlamına gelir.

**Adım 1 — Şimdi (zaten elimizde, sadece görünürlüğü artırılacak):**
Gap analizi + roadmap + maaş etkili öneriler. Bunlar yapıldı, tek iş kalan: homepage/pazarlamada bunu öne çıkarmak (Bölüm 5, madde 1-2).

**Adım 2 — Kısa vade (ücretli planı çalıştırmak):**
Bölüm 4'teki kredi+abonelik modelini gerçek ödeme akışına bağlamak. Bu olmadan hiçbir farklılaşma "satılamaz" — önce temel motoru çalıştırmak gerekiyor.

**Adım 3 — Orta vade (veri/güven derinliği):**
Belge ile maaş doğrulama (Bölüm 5'te not edildi) ve/veya B2B2C (bootcamp/outplacement) açılımı — SalaryInsights'ın kurumsal geliri bu yönün gerçek olduğunu kanıtlıyor.

**Adım 4 — Bahis (doğrulanmadan büyük yatırım yapılmaz): Sesli AI**
Gerçekten hiçbir rakipte yok — SalaryInsights, LinkedIn, kariyer.net hiçbiri sesli özellik sunmuyor. Olası kullanım: "AI Kariyer Koçu ile Sesli Maaş Müzakeresi Provası" veya sesli mock mülakat — `CareerAiService` zaten kendini "kariyer koçu" olarak tanımlıyor, doğal bir uzantı.

Ama önce şu sorular cevaplanmalı:
- **Maliyet:** Realtime sesli AI, metin tabanlı GPT çağrısından çok daha pahalı (dakika başı ücretlendirme) — Bölüm 4'teki marj hesabı tamamen değişir.
- **Talep kanıtı:** Şu an hiçbir kullanıcı bunu istemedi; bu bir varsayım, doğrulanmış bir ihtiyaç değil.
- **Mühendislik karmaşıklığı:** Ses akışı, tarayıcı mikrofon izinleri, gerçek zamanlı gecikme — mevcut CQRS/REST mimarisine yeni bir katman demek.

**Önerim:** Adım 4'ü şimdi yapmayın. Roadmap'e "doğrulanacak fikir" olarak not edin; Adım 1-2 gerçek ödeyen kullanıcı getirdiğinde, o kullanıcılara "sesli mülakat provası ister misiniz?" diye sorup talebi ölçtükten sonra yatırım kararı verin.

---

## 7. Şu An Eksik Olan: Homepage

Belirttiğiniz gibi proje henüz bir pazarlama/satış odaklı ana sayfaya sahip değil (mevcut `/` sayfası fonksiyonel ama jenerik bir "Neler Sunuyoruz" şablonu). Yukarıdaki fiyatlandırma kararı netleşmeden homepage tasarımına geçmek erken olur — çünkü homepage'in CTA'sı ("Ücretsiz Başla" vs "Pro'ya Geç"), fiyatlandırma modeline göre şekillenir. Önerim: önce bu fiyatlandırma kararını onaylayın, sonra homepage'i bu pazarlama mesajları etrafında (farklılaşma noktaları + Free/Pro CTA) tasarlayalım.

---

## Sonraki Adım

Bu fiyatlandırma yaklaşımını onaylarsanız:
1. `User.Plan` alanı zaten Free/Pro olarak modellenmiş (domain'de mevcut) — ödeme entegrasyonu (iyzico/Stripe) ve kullanım sayacı (aylık analiz limiti) eklenmesi gerekiyor.
2. Homepage'i yeniden tasarlayıp bu farklılaşma noktalarını ön plana çıkaracak şekilde yazabiliriz.
