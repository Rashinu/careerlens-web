'use client';

import { SalaryShareForm } from '@/components/shared/salary-share-form';

export default function SalarySubmitPage() {
  return (
    <div className="mx-auto max-w-2xl px-4 py-10">
      <h1 className="text-2xl font-bold mb-2">Maaş Verisi Ekle</h1>
      <p className="text-[var(--muted-foreground)] text-sm mb-8">
        Topluluğa katkıda bulunun. Tüm veriler anonimdir.
      </p>
      <SalaryShareForm />
    </div>
  );
}
