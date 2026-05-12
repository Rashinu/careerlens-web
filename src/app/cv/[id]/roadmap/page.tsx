'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { Loader2 } from 'lucide-react';
import apiClient from '@/lib/api-client';
import type { ApiResponse, RoadmapDto } from '@/types/api';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Skeleton } from '@/components/ui/skeleton';
import { ScoreGauge } from '@/components/shared/score-gauge';
import { ApiErrorAlert } from '@/components/shared/api-error-alert';

interface RoadmapFormData {
  targetPosition: string;
}

interface RecommendationItem {
  title?: string;
  description?: string;
  priority?: string;
  timeframe?: string;
}

interface GapItem {
  area?: string;
  currentLevel?: string;
  requiredLevel?: string;
}

function parseJsonSafe<T>(raw: string, fallback: T): T {
  try {
    return JSON.parse(raw) as T;
  } catch {
    return fallback;
  }
}

function GapAnalysisSection({ raw }: { raw: string }) {
  const items = parseJsonSafe<GapItem[]>(raw, []);
  if (items.length === 0) {
    return <p className="text-sm text-[var(--muted-foreground)]">{raw}</p>;
  }
  return (
    <div className="flex flex-col gap-3">
      {items.map((item, i) => (
        <div key={i} className="rounded-lg border border-[var(--border)] p-3">
          <p className="text-sm font-medium">{item.area ?? `Alan ${i + 1}`}</p>
          {item.currentLevel && (
            <p className="text-xs text-[var(--muted-foreground)]">
              Mevcut: {item.currentLevel} → Hedef: {item.requiredLevel}
            </p>
          )}
        </div>
      ))}
    </div>
  );
}

function RecommendationsTimeline({ raw }: { raw: string }) {
  const items = parseJsonSafe<RecommendationItem[]>(raw, []);
  if (items.length === 0) {
    return <p className="text-sm text-[var(--muted-foreground)] whitespace-pre-wrap">{raw}</p>;
  }
  return (
    <div className="flex flex-col gap-4">
      {items.map((item, i) => (
        <div key={i} className="flex gap-4">
          <div className="flex flex-col items-center">
            <div className="flex h-7 w-7 items-center justify-center rounded-full bg-[#2563EB] text-white text-xs font-bold shrink-0">
              {i + 1}
            </div>
            {i < items.length - 1 && <div className="w-px flex-1 bg-[var(--border)] my-1" />}
          </div>
          <div className="pb-4">
            <p className="text-sm font-medium">{item.title ?? `Adım ${i + 1}`}</p>
            {item.description && (
              <p className="text-xs text-[var(--muted-foreground)] mt-0.5">{item.description}</p>
            )}
            {item.timeframe && (
              <span className="inline-block mt-1 text-xs bg-[var(--muted)] px-2 py-0.5 rounded-full">
                {item.timeframe}
              </span>
            )}
          </div>
        </div>
      ))}
    </div>
  );
}

export default function RoadmapPage() {
  const params = useParams<{ id: string }>();
  const [roadmap, setRoadmap] = useState<RoadmapDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [generating, setGenerating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const { register, handleSubmit, formState: { errors } } = useForm<RoadmapFormData>();

  async function fetchRoadmap() {
    setLoading(true);
    setError(null);
    try {
      const res = await apiClient.get<ApiResponse<RoadmapDto>>(
        `/api/cv/${params.id}/roadmap`
      );
      if (res.data.success) {
        setRoadmap(res.data.data);
      }
    } catch (err: unknown) {
      const status = (err as { response?: { status?: number } })?.response?.status;
      if (status !== 404) {
        setError('Yol haritası yüklenirken hata oluştu.');
      }
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    fetchRoadmap();
  }, [params.id]);

  async function onSubmit(data: RoadmapFormData) {
    setGenerating(true);
    try {
      const res = await apiClient.post<ApiResponse<RoadmapDto>>(
        `/api/cv/${params.id}/roadmap`,
        { targetPosition: data.targetPosition }
      );
      if (res.data.success) {
        setRoadmap(res.data.data);
        toast.success('Yol haritanız oluşturuldu!');
      } else {
        toast.error(res.data.error ?? 'Yol haritası oluşturulamadı.');
      }
    } catch {
      toast.error('Yol haritası oluşturulurken hata oluştu.');
    } finally {
      setGenerating(false);
    }
  }

  if (loading) {
    return (
      <div className="mx-auto max-w-3xl px-4 py-10 flex flex-col gap-4">
        <Skeleton className="h-8 w-56" />
        <Skeleton className="h-48 w-full" />
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-3xl px-4 py-10">
      <h1 className="text-2xl font-bold mb-8">Kariyer Yol Haritası</h1>

      {error && (
        <div className="mb-6">
          <ApiErrorAlert message={error} onRetry={fetchRoadmap} />
        </div>
      )}

      {!roadmap ? (
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Hedef Pozisyon Belirle</CardTitle>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
              <div className="flex flex-col gap-1.5">
                <Label htmlFor="targetPosition">Hedef Pozisyon</Label>
                <Input
                  id="targetPosition"
                  placeholder="Örn: Senior Backend Developer"
                  {...register('targetPosition', {
                    required: 'Hedef pozisyon zorunludur.',
                  })}
                />
                {errors.targetPosition && (
                  <p className="text-xs text-[var(--destructive)]">
                    {errors.targetPosition.message}
                  </p>
                )}
              </div>
              <Button type="submit" disabled={generating} className="w-full">
                {generating ? (
                  <>
                    <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                    Yol Haritası Oluşturuluyor...
                  </>
                ) : (
                  'Yol Haritası Oluştur'
                )}
              </Button>
            </form>
          </CardContent>
        </Card>
      ) : (
        <div className="flex flex-col gap-6">
          {/* Score */}
          <Card>
            <CardHeader>
              <CardTitle className="text-base">Mevcut Skorunuz</CardTitle>
            </CardHeader>
            <CardContent className="flex items-center gap-8 flex-wrap">
              <ScoreGauge score={roadmap.currentScore} size="lg" />
              <div>
                <p className="text-sm text-[var(--muted-foreground)]">Hedef Pozisyon</p>
                <p className="text-lg font-semibold">{roadmap.targetPosition}</p>
              </div>
            </CardContent>
          </Card>

          {/* Gap Analysis */}
          <Card>
            <CardHeader>
              <CardTitle className="text-base">Gelişim Alanları</CardTitle>
            </CardHeader>
            <CardContent>
              <GapAnalysisSection raw={roadmap.gapAnalysis} />
            </CardContent>
          </Card>

          {/* Recommendations */}
          <Card>
            <CardHeader>
              <CardTitle className="text-base">Önerilen Adımlar</CardTitle>
            </CardHeader>
            <CardContent>
              <RecommendationsTimeline raw={roadmap.recommendations} />
            </CardContent>
          </Card>
        </div>
      )}
    </div>
  );
}
