export default function KosullarPage() {
  return (
    <div className="mx-auto max-w-3xl px-4 py-16 prose prose-invert">
      <h1 className="text-3xl font-bold mb-2">Kullanım Şartları</h1>
      <p className="text-sm text-muted-foreground mb-8">Son güncelleme: 22 Haziran 2026</p>

      <h2 className="text-xl font-semibold mt-8 mb-3">1. Hizmet Tanımı</h2>
      <p className="text-sm leading-relaxed">
        CareerLens, CV analizi, kariyer yol haritası ve anonim maaş karşılaştırması sunan bir
        platformdur. CV analizi ve kariyer yol haritası önerileri yapay zeka tarafından üretilir;
        bu öneriler bilgilendirme amaçlıdır ve profesyonel kariyer danışmanlığının yerini tutmaz.
      </p>

      <h2 className="text-xl font-semibold mt-8 mb-3">2. Hesap Sorumluluğu</h2>
      <p className="text-sm leading-relaxed">
        Hesap bilgilerinizin güvenliğinden siz sorumlusunuz. Yüklediğiniz CV içeriğinin doğruluğundan
        ve paylaştığınız maaş bilgisinin gerçeğe uygunluğundan siz sorumlusunuz.
      </p>

      <h2 className="text-xl font-semibold mt-8 mb-3">3. Ücretsiz ve Ücretli Planlar</h2>
      <p className="text-sm leading-relaxed">
        Platform, ücretsiz bir temel plan ve ek özellikler içeren ücretli plan(lar) sunabilir. Ücretli
        plan detayları ve fiyatlandırma, ilgili sayfada ayrıca belirtilir ve önceden bildirilmeden
        güncellenebilir.
      </p>

      <h2 className="text-xl font-semibold mt-8 mb-3">4. Sorumluluk Sınırlaması</h2>
      <p className="text-sm leading-relaxed">
        Maaş benchmark verileri, kullanıcılar tarafından gönüllü olarak paylaşılan anonim verilere
        dayanır ve istatistiksel bir tahmindir; kesin bir garanti oluşturmaz. AI tarafından üretilen
        kariyer önerileri de tahminidir, nihai kararlar kullanıcıya aittir.
      </p>

      <h2 className="text-xl font-semibold mt-8 mb-3">5. Hesap Sonlandırma</h2>
      <p className="text-sm leading-relaxed">
        Hesabınızı istediğiniz zaman silebilirsiniz. Kötüye kullanım (sahte veri paylaşımı, sistemi
        kötüye kullanma girişimi) tespit edilirse hesabınız askıya alınabilir.
      </p>

      <p className="text-xs text-muted-foreground mt-10 border-t border-border pt-4">
        Bu metin genel bilgilendirme amaçlıdır ve hukuki danışmanlık yerine geçmez. Yayına alınmadan
        önce bir hukuk uzmanına gözden geçirtilmesi önerilir.
      </p>
    </div>
  );
}
