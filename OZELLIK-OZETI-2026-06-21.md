# CareerLens — Yeni Özellikler Özeti

**Tarih:** 21 Haziran 2026
**Kapsam:** Gerçek veri toplama döngüsü, TÜFE enflasyon düzeltmesi, roadmap-maaş entegrasyonu

---

## Arka Plan

SalaryInsights.com.tr rakip analizi sonrası belirlenen 3 öncelikli geliştirme tamamlandı:

1. Gerçek maaş verisi toplama döngüsü (sentetik seed veriye bağımlılığı azaltmak)
2. TÜFE bazlı enflasyon düzeltmesi (rakipte olan, bizde olmayan bir özellik)
3. AI kariyer roadmap'inin maaş verisiyle somut şekilde bağlanması

---

## Genel Akış (Uçtan Uca Test Edildi)

| Ana Sayfa | Kayıt Ol | Giriş Yap |
|---|---|---|
| ![Ana sayfa](screenshots/00-ana-sayfa.png) | ![Kayıt ol](screenshots/07-register.png) | ![Giriş yap](screenshots/08-login.png) |

CV yükleme → analiz → roadmap akışı tamamen yerel ortamda (mock AI servisiyle) uçtan uca çalıştırıldı:

![CV analiz sonucu](screenshots/03-cv-analiz-sonucu.png)

---

## 1. Gerçek Veri Toplama Döngüsü

Kullanıcı, CV'sinden oluşturulan kariyer roadmap'ini gördüğü an — yani platforma en çok değer aldığı, karşılık vermeye en istekli olduğu an — ona anonim maaş paylaşımı öneriliyor. Form, CV'den çıkarılan deneyim/teknoloji bilgisiyle ve hedef pozisyonla **önceden dolduruluyor**, sürtünme minimuma indiriliyor.

![Roadmap sayfası ve gömülü maaş paylaşım kartı](screenshots/04-roadmap-tam-sayfa.png)

Bağımsız `/salary/submit` sayfası da aynı bileşeni kullanıyor (kod tekrarı yok):

![Maaş veri girişi formu](screenshots/02-salary-submit-form.png)

---

## 2. TÜFE Enflasyon Düzeltmesi

Maaş benchmark'ları artık TCMB EVDS API'sinden (TÜFE genel endeks, `TP.FE.OKTG01`) çekilen verilerle enflasyon düzeltmeli olarak hesaplanıyor. Eski tarihli bir maaş kaydı ile yeni bir kayıt artık adil şekilde karşılaştırılabiliyor.

![Maaş karşılaştırma sayfası — P25/P50/P75](screenshots/01-salary-benchmark.png)

**Not:** Geliştirme ortamında gerçek TCMB API çağrısı yapılmıyor (mock servis, çarpan=1.0), bu nedenle ekran görüntüsünde "enflasyon düzeltmeli" rozeti görünmüyor — bu beklenen davranış. Production'da `Tcmb:ApiKey` girildiğinde aktif olacak.

**Sizden gereken aksiyon:** [TCMB EVDS](https://evds2.tcmb.gov.tr/) üzerinden ücretsiz API anahtarı alıp `backend/CareerLens.API/appsettings.json` → `Tcmb:ApiKey` alanına eklemeniz gerekiyor.

---

## 3. Roadmap — Maaş Entegrasyonu

**Düzeltilen bug:** Roadmap üretimi önceden hardcoded `"İstanbul", 3 yıl` benchmark kullanıyordu (kullanıcının gerçek profili dikkate alınmıyordu). Artık CV'den çıkarılan gerçek deneyim yılı ve ülke çapında enflasyon düzeltmeli benchmark kullanılıyor.

**Yeni:** AI artık her öneriye somut bir maaş etkisi tahmini ekliyor (örn. "+₺5.000/ay") — yalnızca gerçek benchmark verisine dayanarak, veri yetersizse tahmin üretmiyor (halüsinasyon koruması).

![Gelişim alanları — maaş aralığı ve güçlü/eksik yönler](screenshots/05-gap-analizi-yakin.png)

Tam sayfa görünüm — gap analizi, maaş etkili öneriler ve maaş paylaşım daveti bir arada:

![Roadmap tam sayfa](screenshots/04-roadmap-tam-sayfa.png)

**Yan keşif:** Bu çalışma sırasında roadmap sayfasının render mantığının AI'ın döndürdüğü gerçek JSON şemasıyla hiç eşleşmediği (önceden hiç doğru görüntülenmemiş olabilecek bir bug) bulundu ve düzeltildi.

---

## Teknik Notlar

- **Backend:** 23 unit test (4 yeni) — hepsi geçiyor. `dotnet build` temiz.
- **Frontend:** TypeScript tip kontrolü temiz.
- **Yan düzeltme:** Depo kökünde yanlışlıkla bulunan (Next.js komutlarının yanlış dizinden çalıştırılmasından kalma) `next-env.d.ts` ve `.next/` artıkları temizlendi — bu, `npm run build`'in sürekli başarısız olmasının kök nedeniydi.
- **Commit:** `2880884` — "feat: anonim maaş paylaşım döngüsü, TÜFE enflasyon düzeltmesi ve roadmap-maaş entegrasyonu"

---

## Sıradaki Öncelik

Pazar analizinde belirlenen 4. madde: **B2B2C açılımı** (bootcamp/eğitim akademilerine white-label API satışı). Bu bir ürün/iş stratejisi kararı, henüz kod gerektirmiyor.
