'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { FileText, CheckCircle, Target, Upload } from 'lucide-react';
import apiClient from '@/lib/api-client';
import type { ApiResponse, DashboardDto } from '@/types/api';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Skeleton } from '@/components/ui/skeleton';
import { Button } from '@/components/ui/button';
import { ScoreGauge } from '@/components/shared/score-gauge';
import { StatusBadge } from '@/components/shared/status-badge';
import { ApiErrorAlert } from '@/components/shared/api-error-alert';

function StatCard({
  icon: Icon,
  label,
  value,
}: {
  icon: React.ElementType;
  label: string;
  value: string | number;
}) {
  return (
    <Card>
      <CardHeader className="flex flex-row items-center justify-between pb-2">
        <CardTitle className="text-sm font-medium text-[var(--muted-foreground)]">
          {label}
        </CardTitle>
        <Icon className="h-4 w-4 text-[var(--muted-foreground)]" />
      </CardHeader>
      <CardContent>
        <div className="text-2xl font-bold">{value}</div>
      </CardContent>
    </Card>
  );
}

function DashboardSkeleton() {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
      {[1, 2, 3].map((i) => (
        <Card key={i}>
          <CardHeader className="pb-2">
            <Skeleton className="h-4 w-32" />
          </CardHeader>
          <CardContent>
            <Skeleton className="h-8 w-16" />
          </CardContent>
        </Card>
      ))}
    </div>
  );
}

export default function DashboardPage() {
  const [data, setData] = useState<DashboardDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  async function fetchDashboard() {
    setLoading(true);
    setError(null);
    try {
      const res = await apiClient.get<ApiResponse<DashboardDto>>('/api/dashboard');
      if (res.data.success) {
        setData(res.data.data);
      } else {
        setError(res.data.error ?? 'Veri alınamadı.');
      }
    } catch {
      setError('Dashboard verisi yüklenirken hata oluştu.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    fetchDashboard();
  }, []);

  return (
    <div className="mx-auto max-w-7xl px-4 py-10">
      <div className="flex items-center justify-between mb-8">
        <h1 className="text-2xl font-bold">Dashboard</h1>
        <Button asChild>
          <Link href="/cv/upload">
            <Upload className="h-4 w-4 mr-2" />
            CV Yükle
          </Link>
        </Button>
      </div>

      {error && (
        <div className="mb-6">
          <ApiErrorAlert message={error} onRetry={fetchDashboard} />
        </div>
      )}

      {loading ? (
        <DashboardSkeleton />
      ) : data ? (
        <div className="flex flex-col gap-8">
          <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <StatCard
              icon={FileText}
              label="Toplam CV"
              value={data.totalCvAnalyses}
            />
            <StatCard
              icon={CheckCircle}
              label="Tamamlanan Analiz"
              value={data.completedAnalyses}
            />
            <StatCard
              icon={Target}
              label="Son Skor"
              value={data.latestRoadmapScore ?? '—'}
            />
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Latest CV */}
            <Card>
              <CardHeader>
                <CardTitle className="text-base">Son CV Durumu</CardTitle>
              </CardHeader>
              <CardContent>
                {data.latestCvStatus ? (
                  <div className="flex items-center gap-3">
                    <StatusBadge status={data.latestCvStatus} />
                    <Link href="/cv" className="text-sm text-[#2563EB] hover:underline">
                      Tüm CV'leri Görüntüle →
                    </Link>
                  </div>
                ) : (
                  <p className="text-sm text-[var(--muted-foreground)]">
                    Henüz CV yüklenmemiş.{' '}
                    <Link href="/cv/upload" className="text-[#2563EB] hover:underline">
                      Şimdi yükle
                    </Link>
                  </p>
                )}
              </CardContent>
            </Card>

            {/* Latest Roadmap */}
            <Card>
              <CardHeader>
                <CardTitle className="text-base">Son Yol Haritası</CardTitle>
              </CardHeader>
              <CardContent>
                {data.latestRoadmapScore !== null ? (
                  <div className="flex items-center gap-6">
                    <ScoreGauge score={data.latestRoadmapScore} size="sm" />
                    <div>
                      <p className="text-sm font-medium">{data.latestTargetPosition}</p>
                      <p className="text-xs text-[var(--muted-foreground)]">Hedef Pozisyon</p>
                    </div>
                  </div>
                ) : (
                  <p className="text-sm text-[var(--muted-foreground)]">
                    Henüz yol haritası oluşturulmamış.
                  </p>
                )}
              </CardContent>
            </Card>
          </div>
        </div>
      ) : null}
    </div>
  );
}
