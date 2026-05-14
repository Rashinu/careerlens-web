import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function formatCurrency(value: number): string {
  return `₺${value.toLocaleString('tr-TR')}`;
}

export function formatDate(iso: string): string {
  return new Date(iso).toLocaleDateString('tr-TR', {
    day: 'numeric',
    month: 'long',
    year: 'numeric',
  });
}

export function getStatusLabel(status: string): string {
  const labels: Record<string, string> = {
    Uploaded: 'Yüklendi',
    TextExtracted: 'Metin Çıkarıldı',
    Analyzed: 'Analiz Tamamlandı',
    Failed: 'Başarısız',
  };
  return labels[status] ?? status;
}
