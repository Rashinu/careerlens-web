'use client';

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { X, Shield, CheckCircle2 } from 'lucide-react';
import apiClient from '@/lib/api-client';
import type { ApiResponse } from '@/types/api';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Badge } from '@/components/ui/badge';

interface SubmitForm {
  position: string;
  sector: string;
  city: string;
  yearsOfExperience: number;
  netSalary: number;
  techTagInput: string;
}

export default function SalarySubmitPage() {
  const [techStack, setTechStack] = useState<string[]>([]);
  const [submitted, setSubmitted] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const {
    register,
    handleSubmit,
    watch,
    setValue,
    formState: { errors },
  } = useForm<SubmitForm>({ defaultValues: { yearsOfExperience: 0, netSalary: 0 } });

  function addTag() {
    const raw = watch('techTagInput')?.trim();
    if (!raw) return;
    const tag = raw.replace(/,/g, '').trim();
    if (tag && !techStack.includes(tag)) {
      setTechStack((prev) => [...prev, tag]);
    }
    setValue('techTagInput', '');
  }

  function removeTag(tag: string) {
    setTechStack((prev) => prev.filter((t) => t !== tag));
  }

  async function onSubmit(data: SubmitForm) {
    setIsLoading(true);
    try {
      const res = await apiClient.post<ApiResponse<null>>('/api/salary/submit', {
        position: data.position,
        sector: data.sector,
        city: data.city,
        yearsOfExperience: data.yearsOfExperience,
        netSalary: data.netSalary,
        techStack,
      });
      if (res.data.success) {
        setSubmitted(true);
        toast.success('Maaş veriniz anonim olarak kaydedildi, teşekkürler!');
      } else {
        toast.error(res.data.error ?? 'Gönderim başarısız.');
      }
    } catch {
      toast.error('Gönderim sırasında hata oluştu.');
    } finally {
      setIsLoading(false);
    }
  }

  if (submitted) {
    return (
      <div className="mx-auto max-w-xl px-4 py-24 flex flex-col items-center text-center gap-4">
        <CheckCircle2 className="h-16 w-16 text-green-500" />
        <h2 className="text-2xl font-bold">Teşekkürler!</h2>
        <p className="text-[var(--muted-foreground)]">
          Maaş veriniz anonim olarak sisteme kaydedildi ve toplu analizlere katkı sağlayacak.
        </p>
        <Button onClick={() => setSubmitted(false)} variant="outline">
          Yeni Veri Ekle
        </Button>
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-2xl px-4 py-10">
      <h1 className="text-2xl font-bold mb-2">Maaş Verisi Ekle</h1>
      <p className="text-[var(--muted-foreground)] text-sm mb-8">
        Topluluğa katkıda bulunun. Tüm veriler anonimdir.
      </p>

      {/* Anonymity note */}
      <div className="flex items-start gap-3 rounded-lg border border-blue-200 bg-blue-50 dark:bg-blue-950 dark:border-blue-800 p-4 mb-6">
        <Shield className="h-5 w-5 text-[#2563EB] shrink-0 mt-0.5" />
        <div>
          <p className="text-sm font-medium text-[#2563EB]">Gizlilik Garantisi</p>
          <p className="text-xs text-[var(--muted-foreground)] mt-0.5">
            Maaş verileriniz hiçbir zaman hesabınızla veya kişisel bilgilerinizle ilişkilendirilmez.
            Veriler yalnızca istatistiksel analizde kullanılır.
          </p>
        </div>
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="text-base">Maaş Bilgileri</CardTitle>
          <CardDescription>Tüm alanlar zorunludur</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-5">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="flex flex-col gap-1.5">
                <Label htmlFor="position">Pozisyon</Label>
                <Input
                  id="position"
                  placeholder="Örn: Senior Developer"
                  {...register('position', { required: 'Zorunlu alan.' })}
                />
                {errors.position && (
                  <p className="text-xs text-[var(--destructive)]">{errors.position.message}</p>
                )}
              </div>
              <div className="flex flex-col gap-1.5">
                <Label htmlFor="sector">Sektör</Label>
                <Input
                  id="sector"
                  placeholder="Örn: Fintech"
                  {...register('sector', { required: 'Zorunlu alan.' })}
                />
                {errors.sector && (
                  <p className="text-xs text-[var(--destructive)]">{errors.sector.message}</p>
                )}
              </div>
              <div className="flex flex-col gap-1.5">
                <Label htmlFor="city">Şehir</Label>
                <Input
                  id="city"
                  placeholder="Örn: İstanbul"
                  {...register('city', { required: 'Zorunlu alan.' })}
                />
                {errors.city && (
                  <p className="text-xs text-[var(--destructive)]">{errors.city.message}</p>
                )}
              </div>
              <div className="flex flex-col gap-1.5">
                <Label htmlFor="yearsOfExperience">Deneyim (Yıl)</Label>
                <Input
                  id="yearsOfExperience"
                  type="number"
                  min={0}
                  placeholder="0"
                  {...register('yearsOfExperience', {
                    required: 'Zorunlu alan.',
                    valueAsNumber: true,
                    min: { value: 0, message: 'Geçerli bir değer girin.' },
                  })}
                />
                {errors.yearsOfExperience && (
                  <p className="text-xs text-[var(--destructive)]">{errors.yearsOfExperience.message}</p>
                )}
              </div>
              <div className="flex flex-col gap-1.5 sm:col-span-2">
                <Label htmlFor="netSalary">Net Maaş (₺)</Label>
                <Input
                  id="netSalary"
                  type="number"
                  min={0}
                  placeholder="Örn: 45000"
                  {...register('netSalary', {
                    required: 'Zorunlu alan.',
                    valueAsNumber: true,
                    min: { value: 1, message: 'Geçerli bir maaş girin.' },
                  })}
                />
                {errors.netSalary && (
                  <p className="text-xs text-[var(--destructive)]">{errors.netSalary.message}</p>
                )}
              </div>
            </div>

            {/* Tech tag input */}
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="techTagInput">Teknoloji Yığını</Label>
              <div className="flex gap-2">
                <Input
                  id="techTagInput"
                  placeholder="Örn: React, Node.js — Enter ile ekle"
                  {...register('techTagInput')}
                  onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                      e.preventDefault();
                      addTag();
                    }
                  }}
                />
                <Button type="button" variant="outline" onClick={addTag}>
                  Ekle
                </Button>
              </div>
              {techStack.length > 0 && (
                <div className="flex flex-wrap gap-2 mt-2">
                  {techStack.map((tag) => (
                    <Badge
                      key={tag}
                      variant="secondary"
                      className="flex items-center gap-1 pr-1"
                    >
                      {tag}
                      <button
                        type="button"
                        onClick={() => removeTag(tag)}
                        aria-label={`${tag} etiketini kaldır`}
                        className="hover:opacity-70"
                      >
                        <X className="h-3 w-3" />
                      </button>
                    </Badge>
                  ))}
                </div>
              )}
            </div>

            <Button type="submit" disabled={isLoading} className="w-full">
              {isLoading ? 'Gönderiliyor...' : 'Anonim Olarak Gönder'}
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
