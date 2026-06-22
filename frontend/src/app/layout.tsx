import type { Metadata } from 'next';
import { ThemeProvider } from 'next-themes';
import { Toaster } from 'sonner';
import { Analytics } from '@vercel/analytics/next';
import { AuthProvider } from '@/context/auth-context';
import { Navbar } from '@/components/layout/navbar';
import './globals.css';

export const metadata: Metadata = {
  title: {
    default: 'CareerLens — AI ile Kariyerini Şekillendir',
    template: '%s | CareerLens',
  },
  description:
    'CV\'ini yükle, maaşının piyasada nerede durduğunu öğren, 12 aylık kariyer yol haritanı al. Türkiye\'nin AI destekli kariyer platformu.',
  keywords: ['kariyer', 'maaş karşılaştırma', 'CV analizi', 'yapay zeka', 'iş', 'Türkiye', 'roadmap'],
  authors: [{ name: 'CareerLens' }],
  openGraph: {
    type: 'website',
    locale: 'tr_TR',
    url: 'https://careerlens-web.vercel.app',
    siteName: 'CareerLens',
    title: 'CareerLens — AI ile Kariyerini Şekillendir',
    description:
      'CV\'ini yükle, maaşının piyasada nerede durduğunu öğren, 12 aylık kariyer yol haritanı al. Türkiye\'nin AI destekli kariyer platformu.',
    images: [
      {
        url: 'https://careerlens-web.vercel.app/og-image.png',
        width: 1200,
        height: 630,
        alt: 'CareerLens — AI Destekli Kariyer Analizi',
      },
    ],
  },
  twitter: {
    card: 'summary_large_image',
    title: 'CareerLens — AI ile Kariyerini Şekillendir',
    description: 'CV analizi, maaş karşılaştırma ve kişisel kariyer yol haritası.',
    images: ['https://careerlens-web.vercel.app/og-image.png'],
  },
  metadataBase: new URL('https://careerlens-web.vercel.app'),
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="tr" suppressHydrationWarning>
      <body className="min-h-screen flex flex-col">
        <ThemeProvider attribute="class" defaultTheme="system" enableSystem>
          <AuthProvider>
            <Navbar />
            <main className="flex-1">{children}</main>
            <Toaster richColors position="top-right" />
          </AuthProvider>
        </ThemeProvider>
        <Analytics />
      </body>
    </html>
  );
}
