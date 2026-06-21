'use client';

import { useState } from 'react';
import Link from 'next/link';
import { useForm } from 'react-hook-form';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { Info, PlusCircle } from 'lucide-react';
import apiClient from '@/lib/api-client';
import type { ApiResponse, SalaryBenchmarkDto } from '@/types/api';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { ApiErrorAlert } from '@/components/shared/api-error-alert';
import { formatCurrency } from '@/lib/utils';

interface FilterForm {
  position: string;
  city: string;
  years: number;
}

function PercentileCard({
  label,
  value,
  color,
}: {
  label: string;
  value: number;
  color: string;
}) {
  return (
    <Card>
      <CardHeader className="pb-2">
        <CardTitle className="text-sm font-medium text-[var(--muted-foreground)]">
          {label}
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p className="text-2xl font-bold" style={{ color }}>
          {formatCurrency(value)}
        </p>
      </CardContent>
    </Card>
  );
}

export default function SalaryPage() {
  const [benchmark, setBenchmark] = useState<SalaryBenchmarkDto | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    getValues,
    formState: { errors },
  } = useForm<FilterForm>({ defaultValues: { years: 0 } });

  async function onSubmit(data: FilterForm) {
    setLoading(true);
    setError(null);
    try {
      const res = await apiClient.get<ApiResponse<SalaryBenchmarkDto>>(
        '/api/salary/benchmark',
        {
          params: {
            position: data.position,
            city: data.city,
            years: data.years,
          },
        }
      );
      if (res.data.success) {
        setBenchmark(res.data.data);
      } else {
        setError(res.data.error ?? 'Veri alınamadı.');
      }
    } catch {
      setError('Maaş verisi yüklenirken hata oluştu.');
    } finally {
      setLoading(false);
    }
  }

  const chartData = benchmark
    ? [
        { name: 'P25 (Alt)', value: benchmark.p25, fill: '#3b82f6' },
        { name: 'P50 (Medyan)', value: benchmark.p50, fill: '#2563EB' },
        { name: 'P75 (Üst)', value: benchmark.p75, fill: '#1d4ed8' },
      ]
    : [];

  return (
    <div className="mx-auto max-w-5xl px-4 py-10">
      <div className="flex items-start justify-between flex-wrap gap-4 mb-8">
        <div>
          <h1 className="text-2xl font-bold">Maaş Karşılaştırma</h1>
          <p className="text-sm text-[var(--muted-foreground)] mt-1">
            Anonim piyasa verilerine dayalı maaş dağılımı
          </p>
        </div>
        <Button variant="outline" asChild>
          <Link href="/salary/submit">
            <PlusCircle className="h-4 w-4 mr-2" />
            Maaş Verisi Ekle
          </Link>
        </Button>
      </div>

      {/* Filter form */}
      <Card className="mb-8">
        <CardHeader>
          <CardTitle className="text-base">Filtrele</CardTitle>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)} className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="position">Pozisyon</Label>
              <Input
                id="position"
                placeholder="Örn: Backend Developer"
                {...register('position', { required: 'Pozisyon zorunludur.' })}
              />
              {errors.position && (
                <p className="text-xs text-[var(--destructive)]">{errors.position.message}</p>
              )}
            </div>
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="city">Şehir</Label>
              <Input
                id="city"
                placeholder="Örn: İstanbul"
                {...register('city', { required: 'Şehir zorunludur.' })}
              />
              {errors.city && (
                <p className="text-xs text-[var(--destructive)]">{errors.city.message}</p>
              )}
            </div>
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="years">Deneyim (Yıl)</Label>
              <Input
                id="years"
                type="number"
                min={0}
                placeholder="0"
                {...register('years', { valueAsNumber: true, min: 0 })}
              />
            </div>
            <div className="sm:col-span-3">
              <Button type="submit" disabled={loading} className="w-full sm:w-auto">
                {loading ? (
                  <span className="flex items-center gap-2">
                    <svg className="animate-spin h-4 w-4" viewBox="0 0 24 24" fill="none">
                      <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"/>
                      <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8z"/>
                    </svg>
                    Sunucu uyanıyor...
                  </span>
                ) : 'Karşılaştır'}
              </Button>
              {loading && (
                <p className="text-xs text-muted-foreground mt-2">
                  İlk istek 10-30 saniye sürebilir. Lütfen bekleyin.
                </p>
              )}
            </div>
          </form>
        </CardContent>
      </Card>

      {error && (
        <div className="mb-6">
          <ApiErrorAlert
            message={error}
            onRetry={() => handleSubmit(onSubmit)(getValues() as unknown as React.BaseSyntheticEvent)}
          />
        </div>
      )}

      {benchmark && (
        <div className="flex flex-col gap-6">
          {/* Percentile cards */}
          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <PercentileCard label="P25 — Alt Dilim" value={benchmark.p25} color="#3b82f6" />
            <PercentileCard label="P50 — Medyan" value={benchmark.p50} color="#2563EB" />
            <PercentileCard label="P75 — Üst Dilim" value={benchmark.p75} color="#1d4ed8" />
          </div>

          {/* Bar chart */}
          <Card>
            <CardHeader className="flex flex-row items-center justify-between gap-2">
              <CardTitle className="text-base">
                {benchmark.position} — {benchmark.city} ({benchmark.yearsOfExperience} yıl deneyim)
              </CardTitle>
              {benchmark.isInflationAdjusted && (
                <span className="text-xs font-medium text-[#2563EB] bg-blue-50 dark:bg-blue-950 px-2 py-1 rounded-full shrink-0">
                  TÜFE ile enflasyon düzeltmeli
                </span>
              )}
            </CardHeader>
            <CardContent>
              <ResponsiveContainer width="100%" height={260}>
                <BarChart data={chartData} margin={{ top: 8, right: 24, left: 24, bottom: 8 }}>
                  <CartesianGrid strokeDasharray="3 3" stroke="var(--border)" />
                  <XAxis dataKey="name" tick={{ fontSize: 12 }} />
                  <YAxis
                    tickFormatter={(v: number) => `₺${(v / 1000).toFixed(0)}K`}
                    tick={{ fontSize: 12 }}
                  />
                  <Tooltip formatter={(val) => formatCurrency(Number(val))} />
                  <Bar dataKey="value" radius={[4, 4, 0, 0]}>
                    {chartData.map((entry, i) => (
                      <rect key={i} fill={entry.fill} />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            </CardContent>
          </Card>

          {/* Sample count note */}
          <div className="flex items-center gap-2 text-xs text-[var(--muted-foreground)]">
            <Info className="h-3.5 w-3.5 shrink-0" />
            <span>
              Bu veriler <strong>{benchmark.sampleCount}</strong> anonim maaş kaydına dayanmaktadır.
              Tüm veriler kişisel bilgi içermez.
            </span>
          </div>
        </div>
      )}
    </div>
  );
}
