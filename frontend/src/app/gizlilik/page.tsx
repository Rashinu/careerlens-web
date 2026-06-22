export default function GizlilikPage() {
  return (
    <div className="mx-auto max-w-3xl px-4 py-16 prose prose-invert">
      <h1 className="text-3xl font-bold mb-2">Gizlilik Politikası</h1>
      <p className="text-sm text-muted-foreground mb-8">Son güncelleme: 22 Haziran 2026</p>

      <h2 className="text-xl font-semibold mt-8 mb-3">1. Hangi Verileri Topluyoruz</h2>
      <ul className="list-disc pl-6 space-y-1 text-sm leading-relaxed">
        <li><strong>Hesap bilgileri:</strong> ad, soyad, e-posta, şifre (hash'lenmiş olarak saklanır).</li>
        <li><strong>CV verisi:</strong> yüklediğiniz CV dosyası ve bu dosyadan yapay zeka ile çıkarılan yapılandırılmış bilgiler (deneyim, teknoloji, eğitim).</li>
        <li><strong>Maaş verisi:</strong> pozisyon, şehir, deneyim, net maaş, teknoloji yığını. Bu veri <strong>hiçbir zaman</strong> hesabınızla veya kimliğinizle ilişkilendirilmez; tamamen anonim olarak saklanır.</li>
      </ul>

      <h2 className="text-xl font-semibold mt-8 mb-3">2. Verilerinizi Nasıl Kullanıyoruz</h2>
      <p className="text-sm leading-relaxed">
        CV'niz, kariyer yol haritası ve gap analizi üretmek için yapay zeka servis sağlayıcısına (OpenAI)
        gönderilir. Maaş verileriniz, anonim piyasa karşılaştırmaları (P25/P50/P75 yüzdelik dilimleri)
        hesaplamak için kullanılır. Enflasyon düzeltmesi için TCMB'nin halka açık TÜFE verisi kullanılır —
        bu süreç kişisel veri içermez.
      </p>

      <h2 className="text-xl font-semibold mt-8 mb-3">3. Üçüncü Taraf Hizmet Sağlayıcılar</h2>
      <ul className="list-disc pl-6 space-y-1 text-sm leading-relaxed">
        <li>OpenAI (CV analizi ve kariyer roadmap üretimi için)</li>
        <li>Azure Blob Storage / yerel depolama (CV dosyalarının güvenli saklanması için)</li>
        <li>TCMB EVDS (enflasyon endeksi verisi — kişisel veri içermez)</li>
      </ul>

      <h2 className="text-xl font-semibold mt-8 mb-3">4. Haklarınız (KVKK)</h2>
      <p className="text-sm leading-relaxed">
        6698 sayılı Kişisel Verilerin Korunması Kanunu kapsamında verilerinize erişme, düzeltme,
        silinmesini talep etme haklarına sahipsiniz. Talepleriniz için bizimle iletişime geçebilirsiniz.
      </p>

      <h2 className="text-xl font-semibold mt-8 mb-3">5. Veri Saklama ve Silme</h2>
      <p className="text-sm leading-relaxed">
        Hesabınızı sildiğinizde CV dosyalarınız ve kişisel bilgileriniz sistemden kaldırılır. Anonim
        maaş kayıtları, hesabınızla bağlantısı olmadığı için bu işlemden etkilenmez ve istatistiksel
        veri setinde kalmaya devam eder.
      </p>

      <p className="text-xs text-muted-foreground mt-10 border-t border-border pt-4">
        Bu metin genel bilgilendirme amaçlıdır ve hukuki danışmanlık yerine geçmez. Yayına alınmadan
        önce bir hukuk uzmanına gözden geçirtilmesi önerilir.
      </p>
    </div>
  );
}
