import Link from 'next/link';
import { FileSearch, BarChart2, Map, ArrowRight, CheckCircle } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '@/components/ui/card';

const features = [
  {
    icon: FileSearch,
    title: 'CV Analizi',
    description:
      'Yapay zeka CV\'nizi saniyeler içinde analiz eder, güçlü ve gelişime açık yönlerinizi belirler.',
  },
  {
    icon: BarChart2,
    title: 'Maaş Karşılaştırma',
    description:
      'Pozisyon, şehir ve deneyime göre gerçek piyasa verilerini anonim olarak karşılaştırın.',
  },
  {
    icon: Map,
    title: 'Kariyer Yol Haritası',
    description:
      'Hedef pozisyonunuza ulaşmak için kişiselleştirilmiş adım adım öneri planı alın.',
  },
];

const steps = [
  { number: '01', title: 'CV Yükle', desc: 'PDF veya DOCX formatında CV\'nizi yükleyin.' },
  { number: '02', title: 'Analizi İzle', desc: 'AI sistemi CV\'nizi analiz eder ve sizi değerlendirir.' },
  { number: '03', title: 'Yol Haritanı Al', desc: 'Hedef pozisyon belirle ve kişisel plan oluştur.' },
];

export default function HomePage() {
  return (
    <div className="flex flex-col">
      {/* Hero */}
      <section className="mx-auto max-w-7xl px-4 py-24 text-center">
        <span className="inline-block rounded-full bg-blue-50 px-4 py-1 text-sm font-medium text-[#2563EB] dark:bg-blue-950 dark:text-blue-300 mb-6">
          Türkiye pazarı odaklı AI kariyer platformu
        </span>
        <h1 className="text-4xl font-extrabold tracking-tight sm:text-5xl lg:text-6xl mb-6">
          Kariyerinizi{' '}
          <span className="text-[#2563EB]">AI ile şekillendirin</span>
        </h1>
        <p className="mx-auto max-w-2xl text-lg text-[var(--muted-foreground)] mb-10">
          CV analizinden maaş karşılaştırmasına, kariyer yol haritasına kadar — tüm kariyer kararlarınız için veriye dayalı içgörüler.
        </p>
        <div className="flex flex-col sm:flex-row gap-4 justify-center">
          <Button size="lg" asChild>
            <Link href="/register">
              Hemen Başla <ArrowRight className="h-4 w-4 ml-1" />
            </Link>
          </Button>
          <Button size="lg" variant="outline" asChild>
            <Link href="/salary">Maaş Karşılaştır</Link>
          </Button>
        </div>
      </section>

      {/* Features */}
      <section className="bg-[var(--muted)] py-20">
        <div className="mx-auto max-w-7xl px-4">
          <h2 className="text-3xl font-bold text-center mb-12">Neler Sunuyoruz?</h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            {features.map((f) => (
              <Card key={f.title} className="hover:shadow-lg transition-shadow">
                <CardHeader>
                  <div className="h-10 w-10 rounded-lg bg-[#2563EB]/10 flex items-center justify-center mb-3">
                    <f.icon className="h-5 w-5 text-[#2563EB]" />
                  </div>
                  <CardTitle>{f.title}</CardTitle>
                </CardHeader>
                <CardContent>
                  <CardDescription className="text-sm leading-relaxed">
                    {f.description}
                  </CardDescription>
                </CardContent>
              </Card>
            ))}
          </div>
        </div>
      </section>

      {/* How it works */}
      <section className="mx-auto max-w-7xl px-4 py-20">
        <h2 className="text-3xl font-bold text-center mb-12">Nasıl Çalışır?</h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          {steps.map((step) => (
            <div key={step.number} className="flex flex-col items-center text-center gap-3">
              <span className="text-4xl font-extrabold text-[#2563EB]/20">{step.number}</span>
              <CheckCircle className="h-8 w-8 text-[#2563EB] -mt-4" />
              <h3 className="text-lg font-semibold">{step.title}</h3>
              <p className="text-[var(--muted-foreground)] text-sm">{step.desc}</p>
            </div>
          ))}
        </div>
      </section>

      {/* CTA Strip */}
      <section className="bg-[#2563EB] py-16 text-center">
        <h2 className="text-3xl font-bold text-white mb-4">
          Bugün başla, kariyerini hızlandır
        </h2>
        <p className="text-blue-100 mb-8">
          Ücretsiz plan ile CV analizini hemen dene.
        </p>
        <Button variant="outline" size="lg" asChild className="bg-white text-[#2563EB] hover:bg-blue-50 border-white">
          <Link href="/register">Ücretsiz Hesap Oluştur</Link>
        </Button>
      </section>

      {/* Footer */}
      <footer className="border-t border-[var(--border)] py-8 text-center text-sm text-[var(--muted-foreground)]">
        <div className="mx-auto max-w-7xl px-4 flex flex-col sm:flex-row items-center justify-between gap-2">
          <span>
            Career<strong>Lens</strong> &copy; {new Date().getFullYear()}
          </span>
          <div className="flex gap-4">
            <Link href="/salary" className="hover:underline">Maaş Karşılaştırma</Link>
            <Link href="/login" className="hover:underline">Giriş Yap</Link>
          </div>
        </div>
      </footer>
    </div>
  );
}
