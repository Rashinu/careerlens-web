import Link from "next/link";
import {
  ArrowRight,
  Upload,
  Sparkles,
  MapPin,
  FileSearch,
  BarChart2,
  Map,
  ChevronRight,
} from "lucide-react";
import { Button } from "@/components/ui/button";

const features = [
  {
    icon: FileSearch,
    title: "CV Analizi",
    description:
      "Yapay zeka CV'nizi saniyeler icinde analiz eder, guclu ve gelisime acik yonlerinizi belirler.",
    color: "text-blue-500",
    bg: "bg-blue-500/10",
  },
  {
    icon: BarChart2,
    title: "Maas Karsilastirma",
    description:
      "Pozisyon, sehir ve deneyime gore gercek piyasa verilerini anonim olarak karsilastirin.",
    color: "text-violet-500",
    bg: "bg-violet-500/10",
  },
  {
    icon: Map,
    title: "Kariyer Yol Haritasi",
    description:
      "Hedef pozisyonunuza ulasmak icin kisilestirilmis adim adim oneri plani alin.",
    color: "text-emerald-500",
    bg: "bg-emerald-500/10",
  },
];

const steps = [
  {
    number: "01",
    icon: Upload,
    title: "CV Yukle",
    desc: "PDF veya DOCX formatinda CV'nizi surukleyin veya secin.",
    color: "text-blue-500",
    bg: "bg-blue-500/10",
  },
  {
    number: "02",
    icon: Sparkles,
    title: "AI Analiz Etsin",
    desc: "Yapay zeka CV'nizi ayristica analiz eder ve puanlar.",
    color: "text-violet-500",
    bg: "bg-violet-500/10",
  },
  {
    number: "03",
    icon: MapPin,
    title: "Yol Haritani Al",
    desc: "Hedef pozisyon belirle, kisisel kariyer planinizi gorun.",
    color: "text-emerald-500",
    bg: "bg-emerald-500/10",
  },
];

export default function HomePage() {
  return (
    <div className="flex flex-col">
      {/* ── Hero ── */}
      <section className="relative overflow-hidden">
        {/* Animated gradient blobs */}
        <div
          aria-hidden
          className="pointer-events-none absolute inset-0 -z-10"
        >
          <div className="animate-blob animation-delay-0 absolute -top-40 -left-40 h-96 w-96 rounded-full bg-blue-500/20 blur-3xl" />
          <div className="animate-blob animation-delay-2000 absolute top-20 right-0 h-80 w-80 rounded-full bg-violet-500/15 blur-3xl" />
          <div className="animate-blob animation-delay-4000 absolute bottom-0 left-1/2 h-72 w-72 rounded-full bg-emerald-500/10 blur-3xl" />
        </div>

        <div className="mx-auto max-w-5xl px-4 py-28 text-center">
          <span className="mb-6 inline-flex items-center gap-1.5 rounded-full border border-blue-200 bg-blue-50 px-4 py-1.5 text-sm font-medium text-blue-700 dark:border-blue-800 dark:bg-blue-950/60 dark:text-blue-300">
            <Sparkles className="h-3.5 w-3.5" />
            Turkiye pazari odakli AI kariyer platformu
          </span>

          <h1 className="mb-6 text-5xl font-extrabold tracking-tight text-foreground sm:text-6xl lg:text-7xl">
            Kariyerinizi{" "}
            <span className="bg-gradient-to-r from-blue-500 via-violet-500 to-blue-600 bg-clip-text text-transparent">
              AI ile sekillendirin
            </span>
          </h1>

          <p className="mx-auto mb-10 max-w-2xl text-lg leading-relaxed text-muted-foreground">
            CV analizinden maas karsilastirmasina, kariyer yol haritasina
            kadar — tum kariyer kararlari icin veriye dayali icgorular.
          </p>

          <div className="flex flex-col items-center justify-center gap-4 sm:flex-row">
            <Button size="lg" asChild className="gap-2 px-8 shadow-lg shadow-blue-500/25">
              <Link href="/register">
                Hemen Basla
                <ArrowRight className="h-4 w-4" />
              </Link>
            </Button>
            <Button size="lg" variant="outline" asChild className="gap-2 px-8">
              <Link href="/salary">
                Maas Karsilastir
                <ChevronRight className="h-4 w-4" />
              </Link>
            </Button>
          </div>

          <p className="mt-6 text-sm text-muted-foreground">
            Ucretsiz basla &mdash; kredi karti gerekmez
          </p>
        </div>
      </section>

      {/* ── Features ── */}
      <section className="bg-muted/40 py-24">
        <div className="mx-auto max-w-6xl px-4">
          <div className="mb-14 text-center">
            <h2 className="mb-3 text-3xl font-bold text-foreground sm:text-4xl">
              Neler Sunuyoruz?
            </h2>
            <p className="mx-auto max-w-xl text-muted-foreground">
              Tek platformda CV analizi, piyasa maasi ve kisisel kariyer
              kocu.
            </p>
          </div>

          <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
            {features.map((f) => (
              <div
                key={f.title}
                className="group rounded-2xl border border-border bg-card p-6 shadow-sm transition-all duration-300 hover:-translate-y-1 hover:shadow-md hover:border-blue-200 dark:hover:border-blue-800"
              >
                <div
                  className={`mb-4 flex h-12 w-12 items-center justify-center rounded-xl ${f.bg}`}
                >
                  <f.icon className={`h-6 w-6 ${f.color}`} />
                </div>
                <h3 className="mb-2 text-lg font-semibold text-card-foreground">
                  {f.title}
                </h3>
                <p className="text-sm leading-relaxed text-muted-foreground">
                  {f.description}
                </p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ── How it works ── */}
      <section id="how-it-works" className="py-24">
        <div className="mx-auto max-w-5xl px-4">
          <div className="mb-14 text-center">
            <h2 className="mb-3 text-3xl font-bold text-foreground sm:text-4xl">
              Nasil Calisir?
            </h2>
            <p className="text-muted-foreground">
              Uc adimda kariyer analizinizi tamamlayin.
            </p>
          </div>

          <div className="relative grid grid-cols-1 gap-10 md:grid-cols-3">
            {/* Connector line (desktop only) */}
            <div
              aria-hidden
              className="absolute top-8 left-1/6 right-1/6 hidden h-px bg-gradient-to-r from-blue-200 via-violet-200 to-emerald-200 dark:from-blue-900 dark:via-violet-900 dark:to-emerald-900 md:block"
            />

            {steps.map((step) => (
              <div
                key={step.number}
                className="relative flex flex-col items-center text-center"
              >
                <div className="relative mb-5">
                  <span className="absolute -top-3 -right-3 text-3xl font-extrabold leading-none text-muted-foreground/20">
                    {step.number}
                  </span>
                  <div
                    className={`flex h-16 w-16 items-center justify-center rounded-2xl border-2 border-border ${step.bg} shadow-sm`}
                  >
                    <step.icon className={`h-7 w-7 ${step.color}`} />
                  </div>
                </div>
                <h3 className="mb-2 text-lg font-semibold text-foreground">
                  {step.title}
                </h3>
                <p className="text-sm leading-relaxed text-muted-foreground">
                  {step.desc}
                </p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ── CTA Strip ── */}
      <section className="relative overflow-hidden py-20">
        <div
          aria-hidden
          className="pointer-events-none absolute inset-0 bg-gradient-to-br from-blue-600 via-blue-700 to-violet-700"
        />
        <div
          aria-hidden
          className="pointer-events-none absolute inset-0 opacity-20"
          style={{
            backgroundImage:
              "radial-gradient(circle at 20% 80%, #7c3aed 0%, transparent 50%), radial-gradient(circle at 80% 20%, #2563eb 0%, transparent 50%)",
          }}
        />
        <div className="relative mx-auto max-w-2xl px-4 text-center">
          <h2 className="mb-4 text-3xl font-bold text-white sm:text-4xl">
            Bugun basla, kariyerini hizlandir
          </h2>
          <p className="mb-8 text-blue-100">
            Ucretsiz plan ile CV analizini hemen dene.
          </p>
          <Button
            size="lg"
            asChild
            className="bg-white text-blue-700 hover:bg-blue-50 border-0 shadow-xl shadow-blue-900/30 px-8"
          >
            <Link href="/register">Ucretsiz Hesap Olustur</Link>
          </Button>
        </div>
      </section>

      {/* ── Footer ── */}
      <footer className="border-t border-border py-8">
        <div className="mx-auto max-w-7xl px-4 flex flex-col items-center justify-between gap-4 sm:flex-row">
          <Link href="/" className="flex items-center gap-1 font-bold text-foreground">
            Career<span className="text-blue-500">Lens</span>
            <span className="h-1.5 w-1.5 rounded-full bg-blue-500" aria-hidden />
          </Link>
          <p className="text-sm text-muted-foreground">
            &copy; {new Date().getFullYear()} CareerLens. Tum haklari saklidir.
          </p>
          <div className="flex gap-5 text-sm text-muted-foreground">
            <Link href="/salary" className="hover:text-foreground transition-colors">
              Maas Karsilastirma
            </Link>
            <Link href="/login" className="hover:text-foreground transition-colors">
              Giris Yap
            </Link>
          </div>
        </div>
      </footer>
    </div>
  );
}
