import type { Metadata } from 'next';
import { ThemeProvider } from 'next-themes';
import { Toaster } from 'sonner';
import { AuthProvider } from '@/context/auth-context';
import { Navbar } from '@/components/layout/navbar';
import './globals.css';

export const metadata: Metadata = {
  title: 'CareerLens — AI Destekli Kariyer Analizi',
  description:
    'CV analizi, maaş karşılaştırma ve kariyer yol haritası. Türkiye pazarı odaklı.',
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
      </body>
    </html>
  );
}
